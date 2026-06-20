using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PostsVerify.Poc.Api.Domain.Services;
using PostsVerify.Poc.Api.Infrastructure.InMemory;

namespace PostsVerify.Poc.Api.Application;

internal class AccuracyService(
    IRepository repo,
    IHandleNewRatingsService handleNewRatingsService,
    IAnalyticalRepository analyticalRepo) : IAccuracyService
{
    private IHandleNewRatingsService _handleNewRatingsService = handleNewRatingsService;

    public Task Get(int id)
    {
        throw new NotImplementedException();
    }
    public async Task SetValues(Collection<int> ratedPostIds)
    {
        // Get Raters and Posts and Ratings from DB
        var bipartiteNetwork = await repo.GetNetworkFromPosts(ratedPostIds);
        var cachedData = await repo.GetCachedData(bipartiteNetwork.Users);
        var usersAggregations = await analyticalRepo.GetUserAggregations(bipartiteNetwork.Users.Select(x => x.Id).ToList());
        _handleNewRatingsService = new HandleNewRatingsCpService(0.0000001, usersAggregations, bipartiteNetwork, cachedData);
        // Execute the Algorithm Loop and Update Accuracy and Reputation Values
        _handleNewRatingsService.UpdateNetworkScores();

        // Persist the Values in DB
        await repo.SaveNetworkUpdatedValues(bipartiteNetwork);
    }           
}