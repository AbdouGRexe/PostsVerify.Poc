using PostsVerify.Poc.Api.Dtos;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application.Abstractions;

public interface IAddReviewService
{
    Task<int> AddAsync(AddReviewIDto input);
}
