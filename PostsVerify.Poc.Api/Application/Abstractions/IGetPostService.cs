using PostsVerify.Poc.Api.Dtos;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application.Abstractions;

public interface IGetPostService
{
    Task<GetPostTDto> GetAsync(int postId);
    Task<GetPostReviewsTDto> GetReviewsAsync(int postId);
    Task<GetPostReviewTDto> GetReviewAsync(int reviewId);
}
