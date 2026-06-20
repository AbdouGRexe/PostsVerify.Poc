using System.Collections.ObjectModel;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Domain.Services;

internal interface IHandleNewRatingsService
{
    Network UpdateNetworkScores();
}