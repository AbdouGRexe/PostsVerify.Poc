using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PostsVerify.Poc.Api.Domain;
using PostsVerify.Poc.Api.Domain.Values;

namespace PostsVerify.Poc.Api.Infrastructure.InMemory;

public class Repository : IRepository
{
    private readonly InMemoryDb _context = InMemoryDb.GetInstance();
    public Task<Network> GetNetworkFromPosts(Collection<int> postIds)
    {
        try
        {
            return Task.FromResult(new Network
            {
                Users = _context.Users,
                Posts = _context.Posts,
                Ratings = _context.Ratings,
            });
        }
        catch (Exception exception)
        {
            return Task.FromException<Network>(exception);
        }
    }

    public Task SaveNetworkUpdatedValues(object bipartiteNetwork)
    {
        throw new NotImplementedException();
    }

    public Task<UserData> GetCachedData(List<User> bipartiteNetworkUsers)
    {
        throw new NotImplementedException();
    }
}