using PostsVerify.Poc.Api.Dtos;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application.Abstractions;

public interface IGetDataService
{
    Task<UserTDto[]> GetUsersAsync();
    Task<SourceTDto[]> GetSourcesAsync();
    Task<AreaTDto[]> GetAreasAsync();
}
