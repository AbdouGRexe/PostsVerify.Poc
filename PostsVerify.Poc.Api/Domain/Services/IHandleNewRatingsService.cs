using PostsVerify.Poc.Api.Domain.Values;

namespace PostsVerify.Poc.Api.Domain.Services;

internal interface IHandleNewRatingsService
{
    Network UpdateNetworkScores();
}