using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PostsVerify.Poc.Api.Domain.Values;

namespace PostsVerify.Poc.Api.Infrastructure.InMemory;

public interface IAnalyticalRepository
{
    Task<ReadOnlyDictionary<int, UserAggregation>> GetUserAggregations(IEnumerable<int> userIds);

    // ReadOnlyDictionary<Guid, PostAggregation> GetPostAggregation(Guid postId);
    public void SetUserAggregations(UserAggregation userAggregation);
    // public void SetPostAggregation(PostAggregation postAggregation);
}