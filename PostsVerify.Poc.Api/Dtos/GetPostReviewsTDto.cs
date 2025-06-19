using System.Collections.Generic;

namespace PostsVerify.Poc.Api.Dtos;

public class GetPostReviewsTDto
{
    public int VotesScore { get; set; }
    public IEnumerable<ReviewTDto> Reviews { get; init; }
}

public class ReviewTDto
{
    public int Id { get; init; }
    public string User { get; init; }
    public bool Vote { get; init; }
    public string Body { get; set; }
}
