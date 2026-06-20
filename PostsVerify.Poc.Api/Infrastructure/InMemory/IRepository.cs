using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PostsVerify.Poc.Api.Domain;
using PostsVerify.Poc.Api.Domain.Values;

namespace PostsVerify.Poc.Api.Infrastructure.InMemory;

public interface IRepository
{
    Task<Network> GetNetworkFromPosts(Collection<int> postIds);
    Task SaveNetworkUpdatedValues(object bipartiteNetwork);
    Task<UserData> GetCachedData(List<User> bipartiteNetworkUsers);
}