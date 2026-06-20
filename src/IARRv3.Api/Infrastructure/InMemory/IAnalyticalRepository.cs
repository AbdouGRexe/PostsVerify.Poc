using System.Collections.ObjectModel;
using IARRv3.Api.Domain;

namespace IARRv3.Api.Infrastructure.InMemory;

public interface IAnalyticalRepository
{
    Task<ReadOnlyDictionary<Guid, UserAggregation>> GetUserAggregations(IEnumerable<Guid> userIds);

    // ReadOnlyDictionary<Guid, PostAggregation> GetPostAggregation(Guid postId);
    public void SetUserAggregations(UserAggregation userAggregation);
    // public void SetPostAggregation(PostAggregation postAggregation);
}