namespace IARRv3.Api.Domain.Entities;

public class Post(Guid id, double estimatedAccuracyScore)
{
    public Guid Id { get; } = id;
    
    public double EstimatedAccuracyScore { get; set; } = estimatedAccuracyScore;
    
    public double PriorValue {get; set;} = 0.5;
    
    public double? AggregatedData { get; private set; } = null;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsAggregated { get; private set; } = false;
    
    public bool InAggSet { get; private set; } = false;
    
    public Post GetPostCopy()
    {
        return new Post(Id, EstimatedAccuracyScore);
    }

    public static Post GenerateEmpty()
    {
        return new Post(Guid.Empty, 0);
    }
}



