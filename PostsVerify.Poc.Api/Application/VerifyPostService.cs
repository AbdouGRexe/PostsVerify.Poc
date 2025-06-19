using PostsVerify.Poc.Api.Application.Abstractions;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

internal class VerifyPostService : IVerifyPostService
{
    public Task<byte> VerifyAsync(int postId)
    {
        throw new System.NotImplementedException();
    }
}


