using Microsoft.EntityFrameworkCore;
using PostsVerify.Poc.Api.Application.Abstractions;
using PostsVerify.Poc.Api.Dtos;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

public class GetDataService : IGetDataService
{
    private readonly PostsVerifyDbContext _context;

    public GetDataService(PostsVerifyDbContext context)
    {
        _context = context;
    }

    public Task<UserTDto[]> GetUsersAsync()
    {
        return _context.Users
            .Select(user => new UserTDto
            {
                Id = user.Id,
                Label = user.Label,
                Area = user.Area.Label,
                Score = user.Score
            })
            .ToArrayAsync();
    }

    public Task<SourceTDto[]> GetSourcesAsync()
    {
        return _context.Sources
            .Select(source => new SourceTDto
            {
                Id = source.Id,
                Label = source.Label,
                Link = source.Link,
                Score = source.Score
            })
            .ToArrayAsync();
    }

    public Task<AreaTDto[]> GetAreasAsync()
    {
        return _context.Areas
            .Select(area => new AreaTDto
            {
                Id = area.Id,
                Label = area.Label
            })
            .ToArrayAsync();
    }
}
