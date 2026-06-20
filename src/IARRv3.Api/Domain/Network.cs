using System.Collections.ObjectModel;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Domain;

public class Network
{
    public List<User> Users { get; init; }
    
    public List<Post> Posts { get; init; }
    
    public List<Rating> Ratings { get; init; }
    
    public List<Rating> GetRatingsForPost(Guid postId) =>
        Ratings.Where(r => r.PostId == postId).ToList();

    public List<User> GetUsersForPost(Guid postId)
    {
        return Posts
            .Where(post => post.Id == postId)
            .Join(Ratings, post => post.Id, rating => rating.PostId, (_, rating) => rating)
            .Join(Users, postRating => postRating.UserId, user => user.Id, (_, user) => user)
            .ToList();
    }

    public List<Post> GetPostsForUser(Guid userId, List<Guid> qualifiedPostsIds = null)
    {
        var posts = Posts;
        if (qualifiedPostsIds != null)
        {
            posts = posts
                .Join(qualifiedPostsIds, post => post.Id, postId => postId, (post, _) => post)
                .ToList();
        }
        return Users
            .Where(user => user.Id == userId)
            .Join(Ratings, user => user.Id, rating => rating.UserId, (_, r) => r)
            .Join(posts, userRating => userRating.PostId, post => post.Id, (_, p) => p).ToList();
    }

    public List<Rating> GetRatingsForUser(Guid userId) =>
        Ratings.Where(rating => rating.UserId == userId).ToList();

    public double GetSummedUserReputationsForPost(Guid postId)
    {
        return GetUsersForPost(postId)
            .Select(u => u.ReputationScore)
            .Sum();
    }

    public double GetSummedUserWeighedRatingsForPost(Guid postId) =>
        GetRatingsForPost(postId).Join(Users, rating => rating.UserId, user => user.Id, (rating, user) => user.ReputationScore * rating.RatingValue).Sum();

    public double GetCorrAverageForUser(Guid userId, List<Guid> qualifiedPostsIds = null)
    {
        var userRatings = GetRatingsForUser(userId);
        if (qualifiedPostsIds != null)
        {
            userRatings = userRatings
                .Join(qualifiedPostsIds, rating => rating.PostId, postId => postId, (rating, _) => rating)
                .ToList();
        }

        if (userRatings.Count == 0) return 1;
        return userRatings
            .Select(r => r.PreCorrelationValue)
            .Average();
    }
    public double GetPostsAverageForUser(Guid userId, List<Guid> qualifiedPostsIds = null)
    {

        var userPosts = GetPostsForUser(userId, null);
        if (qualifiedPostsIds != null)
        {
            userPosts = userPosts
                .Join(qualifiedPostsIds, post => post.Id, postId => postId, (post, _) => post)
                .ToList();
        }
        if (userPosts.Count == 0) return 1;
        return userPosts.Select(post => post.EstimatedAccuracyScore).Average();
    }

    public double GetRatingsAverageForUser(Guid userId)
    {
        var userRatings = GetRatingsForUser(userId);
        if (userRatings.Count == 0) return 1;
        return userRatings.Average(rating => rating.RatingValue);
    }

    public double GetRatingsStderrForUser(Guid userId)
    {
        var  userRatings = GetRatingsForUser(userId);
        if(userRatings.Count == 0) return 1;
        return StatisticalOperations.GetStderr(userRatings.Select(userRating => userRating.RatingValue)
            .ToList());
    }

    public double GetQualitiesStderrForUser(Guid userId, List<Guid> qualifiedPostsIds = null)
    {
        var userPosts = GetPostsForUser(userId, null);
        if (qualifiedPostsIds != null)
        {
            userPosts = userPosts
                .Join(qualifiedPostsIds, post => post.Id, postId => postId, (post, _) => post)
                .ToList();
        }
        if (userPosts.Count == 0) return 1;
        return StatisticalOperations.GetStderr(userPosts.Select(post => post.EstimatedAccuracyScore)
            .ToList());
    }

    public double GetAveragePostReps()
    {
        var sumsOfRepsAveragePerPost = Users
            .Join(Ratings, user => user.Id, rating => rating.UserId, (user, rating) => new {rating.PostId, user.ReputationScore})
            .GroupBy(rep => rep.PostId, (_, reps) => reps.Sum(rep => rep.ReputationScore));
        return sumsOfRepsAveragePerPost.Average();
    }

    public double GetPostReps(Guid postId)
    {
        return GetUsersForPost(postId).Sum(user => user.ReputationScore);
    }
}