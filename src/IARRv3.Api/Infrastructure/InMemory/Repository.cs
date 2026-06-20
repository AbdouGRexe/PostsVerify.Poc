using System.Collections.ObjectModel;
using IARRv3.Api.Domain;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Infrastructure.InMemory;

public class Repository : IRepository
{
    private readonly InMemoryDb _context = InMemoryDb.GetInstance();
    public Task<Network> GetNetworkFromPosts(Collection<Guid> postIds)
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