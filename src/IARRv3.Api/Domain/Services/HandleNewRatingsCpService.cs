using IARRv3.Api.Domain.Entities;
namespace IARRv3.Api.Domain.Services;


internal class HandleNewRatingsCpService(
    double stopValue,
    IReadOnlyDictionary<Guid, UserAggregation> userAggregations,
    Network network,
    UserData cachedData)
    : IHandleNewRatingsService
{
    private  readonly Network _extendedNetwork = new()
    {
        Users = network.Users,
        Posts = network.Posts.Union(cachedData.Posts).ToList(),
        Ratings = network.Ratings.Union(cachedData.Ratings).ToList()
    };
    
    private List<Post> GetUpdatedPostsWithEstimatedAccuracy()
    {
        // Console.WriteLine($"\n[GetUpdatedPostsWithEstimatedAccuracy()][Ongoing Posts Scores]: ");
        foreach (var post in _extendedNetwork.Posts)
        {
            var summedReps = _extendedNetwork.GetSummedUserReputationsForPost(post.Id);
            var summedWeighedRatings = _extendedNetwork.GetSummedUserWeighedRatingsForPost(post.Id);
            // Console.Write($"Summed Reps : {summedReps}"); // this is giving a value normally
            // Console.Write($"Summed Weighed Ratings : {summedWeighedRatings}"); //  this is giving 0
            post.EstimatedAccuracyScore = summedReps == 0 ? post.EstimatedAccuracyScore : summedWeighedRatings / summedReps;
            // Console.Write($"[post]{Math.Round(post.EstimatedAccuracyScore, 4)}, ");
        }
        
        return _extendedNetwork.Posts;
    }

    private void UpdateUsersReputation(List<Rating> aggregatedRatings)
    {
        foreach (var user in _extendedNetwork.Users)
        {
            var userAgg = userAggregations[user.Id];
            double corr;
            StatSet overlappedCorrStats = null;

            if (aggregatedRatings != null)
            {
                var overlappedRatings = aggregatedRatings.Where(rating => rating.UserId == user.Id).ToList();
                if(overlappedRatings.Count != 0)
                {
                    overlappedCorrStats = new StatSet(overlappedRatings.Count,
                        overlappedRatings.Average(rating => rating.PreCorrelationValue));
                }
            }
            
            var aggCorrStats = new StatSet(userAgg.SizeOfRatingSample, userAgg.MeanRatingsCorrelation);
            var networkUserRatings = _extendedNetwork.GetRatingsForUser(user.Id);
            if (networkUserRatings.Count != 0)
            {
                var networkCorrStats = new StatSet(_extendedNetwork.GetRatingsForUser(user.Id).Count, _extendedNetwork.GetCorrAverageForUser(user.Id));
                corr = StatisticalOperations.GetSetsCombinedAverage((aggCorrStats, networkCorrStats), overlappedCorrStats);
            }
            else
            {
                corr = 0;
            }
            if (corr <= 0) user.ReputationScore = 0;
            else user.ReputationScore = Math.Pow(corr, 1);
            // Console.Write($"[rep, corr]{user.ReputationScore},{corr} ");
        }
    }

    private void UpdateRatingsCorrelation(
        List<Post> aggregatedPosts,
        List<Rating> aggregatedRatings
    )
    {
        // stderr calc especially should be consistent, should creat a class that clalcs combined sets 
        // Console.WriteLine("\n[UpdateRatingsCorrelation][Ongoing Ratings Corr Score]");
        foreach (var rating in _extendedNetwork.Ratings)
        {
            var userAgg = userAggregations[rating.UserId];
            var ratedPostScore = _extendedNetwork.Posts
                .Where(p => p.Id == rating.PostId)
                .Select(p => p.EstimatedAccuracyScore)
                .FirstOrDefault();
            StatSet overlappedPostsStats = null, overlappedRatingsStats = null;
            if (aggregatedPosts.Count != 0)
            {
                var overlappedRatings = aggregatedRatings.Where(rating1 => rating1.UserId == rating.UserId).ToList();
                var overlappedPosts = overlappedRatings.Join(aggregatedPosts, rating1 => rating1.PostId, post => post.Id, (_, post) => post).ToList();
                if (overlappedPosts.Count != 0)
                {
                    overlappedRatingsStats = new StatSet(
                        overlappedRatings.Count,
                        overlappedRatings.Average(r => r.RatingValue));
                    overlappedPostsStats = new StatSet(
                        overlappedPosts.Count,
                        overlappedPosts.Average(post => post.EstimatedAccuracyScore),
                        StatisticalOperations.GetStderr(overlappedPosts.Select(post => post.EstimatedAccuracyScore).ToList()) 
                    );    
                }
            }

            var aggRatingsStats = new StatSet(
                userAgg.SizeOfRatingSample,
                userAgg.MeanOfRatings,
                userAgg.StandardDeviationOfRatings);
            var networkRatingsStats = new StatSet(
                _extendedNetwork.GetRatingsForUser(rating.UserId).Count,
                _extendedNetwork.GetRatingsAverageForUser(rating.UserId),
                _extendedNetwork.GetRatingsStderrForUser(rating.UserId));
            var aggPostsStats = new StatSet(
                userAgg.SizeOfRatingSample, userAgg.MeanOfQualities,
                userAgg.StandardDeviationOfQualities);
            var networkPostsStats = new StatSet(
                _extendedNetwork.GetPostsForUser(rating.UserId, null).Count, 
                _extendedNetwork.GetPostsAverageForUser(rating.UserId),
                _extendedNetwork.GetQualitiesStderrForUser(rating.UserId));


            rating.SetPreCorrelationValue(
                avgRating: StatisticalOperations.GetSetsCombinedAverage((aggRatingsStats, networkRatingsStats), overlappedRatingsStats),
                avgQuality: StatisticalOperations.GetSetsCombinedAverage((aggPostsStats, networkPostsStats), overlappedPostsStats),
                stderrDeviationRating: StatisticalOperations.GetSetsCombinedStderr((aggRatingsStats, networkRatingsStats), overlappedRatingsStats),
                stderrDeviationQuality: StatisticalOperations.GetSetsCombinedStderr((aggPostsStats, networkPostsStats), overlappedPostsStats),
                ratedObjectQuality: ratedPostScore);
        }
        
    }

    private void InitUsersReputation()
    {
        foreach (var user in _extendedNetwork.Users.Where(user => !user.Initiated))
        {
            user.ReputationScore = (float)_extendedNetwork.GetPostsForUser(user.Id, null).Count / _extendedNetwork.Posts.Count; // user is new so all his rated posts are here
            Console.WriteLine($"User {user.Id.ToString()[..5]} initiated as {user.ReputationScore} with number of ratings: {_extendedNetwork.GetRatingsForUser(user.Id).Count}");
            user.Initiated = true;
        }
    }

    public Network UpdateNetworkScores()
    {
        var delta = 1.0;
        InitUsersReputation();
        
        var aggregatedRatings = GetRatingsCopy(_extendedNetwork.Ratings).Where(rating => rating.IsAggregated).ToList();
        var aggregatedPosts = GetPostsCopy(_extendedNetwork.Posts).Where(post => post.IsAggregated).ToList();

        while (delta >= stopValue) // Quality vector stabilizes 
        {
            var currentIter = GetPostsCopy(_extendedNetwork.Posts);
            delta = GetDelta((GetUpdatedPostsWithEstimatedAccuracy(), currentIter));
            UpdateRatingsCorrelation(aggregatedPosts, aggregatedRatings);
            UpdateUsersReputation(aggregatedRatings);
        }
        return _extendedNetwork;
    }

    private static double GetDelta((List<Post> t2, List<Post> t1) tuple)
    {
        var nextVector = tuple.t2;
        var lastVector = tuple.t1;
        var l = nextVector.Count;
        return nextVector.Zip(lastVector,
            (v2, v1) => (float)Math.Pow(v2.EstimatedAccuracyScore - v1.EstimatedAccuracyScore, 2)).Sum() / l;
    }


    
    private static List<Rating> GetRatingsCopy(List<Rating> originalRatings)
    {
        
        return originalRatings.Select(rating => rating.GetRatingCopy()).DefaultIfEmpty().ToList();
    }

    private static List<Post> GetPostsCopy(List<Post> originalPosts)
    {
        return originalPosts.Select(post => post.GetPostCopy()).DefaultIfEmpty().ToList();
    }
}