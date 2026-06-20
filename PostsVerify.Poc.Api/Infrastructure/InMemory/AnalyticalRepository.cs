using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PostsVerify.Poc.Api.Domain.Values;

namespace PostsVerify.Poc.Api.Infrastructure.InMemory;

public class AnalyticalRepository : IAnalyticalRepository
{
    private readonly InMemoryDb _context = InMemoryDb.GetInstance();

    public void SetUserAggregations(UserAggregation userAggregation)
    {
        throw new NotImplementedException();
    }

    public Task<ReadOnlyDictionary<int, UserAggregation>> GetUserAggregations(IEnumerable<int> userIds)
    {
        try
        {
            var aggregations = userIds.ToDictionary(id => id, id => new UserAggregation(id)
            {
                MeanOfRatings = _context.Ratings
                    .Where(rating => rating.UserId == id)
                    .Select(rating => rating.RatingValue)
                    .Average(),
                StandardDeviationOfRatings = 0,
                MeanOfQualities = _context.Users
                    .Where(user => user.Id == id)
                    .Join(_context.Ratings, user => user.Id, rating => rating.UserId, (user, rating) => rating)
                    .Join(_context.Posts, rating => rating.PostId, post => post.Id, (rating, post) => post)
                    .Select(post => post.EstimatedAccuracyScore)
                    .Average(),
                StandardDeviationOfQualities = 0,
                MeanRatingsCorrelation = _context.Ratings.Select(rating => rating.PreCorrelationValue).Average(),
                SizeOfRatingSample = _context.Ratings.Count(rating => rating.UserId == id)
            });

            return Task.FromResult(new ReadOnlyDictionary<int, UserAggregation>(aggregations));
        }
        catch (Exception exception)
        {
            return Task.FromException<ReadOnlyDictionary<int, UserAggregation>>(exception);
        }
    }
}