using System.Collections.ObjectModel;
using IARRv3.Api.Domain;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Infrastructure.InMemory;

public interface IRepository
{
    Task<Network> GetNetworkFromPosts(Collection<Guid> postIds);
    Task SaveNetworkUpdatedValues(object bipartiteNetwork);
    Task<UserData> GetCachedData(List<User> bipartiteNetworkUsers);
}