using System;

namespace PostsVerify.Poc.Api.Domain.Values;

public record UserAggregation(int UserId)
{
    public double MeanOfRatings { get; init; }
    public double StandardDeviationOfRatings { get; init; }
    public double MeanOfQualities { get; init; }
    public double StandardDeviationOfQualities { get; init; }
    public double MeanRatingsCorrelation { get; init; }
    public int SizeOfRatingSample { get; init; }

    public UserAggregation GenerateNewEmpty()
    {
        return new UserAggregation(UserId)
        {
            MeanOfRatings = 1,
            StandardDeviationOfRatings = 1,
            MeanOfQualities = 1,
            StandardDeviationOfQualities = 1,
            MeanRatingsCorrelation = 1,
            SizeOfRatingSample = 0,
        };
    }
}