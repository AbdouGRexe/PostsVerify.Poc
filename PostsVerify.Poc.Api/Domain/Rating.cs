using System;

namespace PostsVerify.Poc.Api.Domain;

public class Rating(int postId, int userId, double ratingValue)
{
    public int PostId { get; set; } = postId;

    public int UserId { get; set; } = userId;

    public double RatingValue { get; private set; } = ratingValue;

    public double PreCorrelationValue { get; private set; }

    public DateTime LastUpdatedIn { get; private set; } = DateTime.Now;

    public (double?, double?) AggregatedData { get; private set; } = (null, null);

    public bool IsAggregated { get; private set; } = false;

    public bool IsDirty { get; private set; } = false;


    public void SetAsAggregated()
    {
        IsAggregated = true;
        IsDirty = false;
    }

    private void SetAsNonAggregated((double, double) aggregatedData)
    {
        AggregatedData = aggregatedData;
        IsAggregated = false;
        IsDirty = true;
    }

    public void SetPreCorrelationValue(double avgRating,
        double avgQuality,
        double stderrDeviationRating,
        double stderrDeviationQuality,
        double ratedObjectQuality)
    {
        if (stderrDeviationRating == 0 || stderrDeviationQuality == 0)
        {
            PreCorrelationValue = 0;
            return;
        }
        SetAsNonAggregated((RatingValue, PreCorrelationValue));
        PreCorrelationValue = (RatingValue - avgRating) / stderrDeviationRating *
                              ((ratedObjectQuality - avgQuality) / stderrDeviationQuality);
    }


    public Rating GetRatingCopy()
    {
        return new Rating(PostId, UserId, RatingValue)
        {
            PreCorrelationValue = PreCorrelationValue
        };
    }
    public void UpdateRating(double updatedRating)
    {
        SetAsNonAggregated((RatingValue, PreCorrelationValue));
        RatingValue = updatedRating;
    }

    public static Rating GenerateEmpty()
    {
        return new Rating(default,  default, 0);
    }
}