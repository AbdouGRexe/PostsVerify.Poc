using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application.Abstractions;

public interface IVerifyPostService
{
    Task<byte> VerifyAsync(int postId);
}


