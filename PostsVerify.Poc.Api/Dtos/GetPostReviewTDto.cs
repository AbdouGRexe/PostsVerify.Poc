namespace PostsVerify.Poc.Api.Dtos;

public class GetPostReviewTDto
{
    public string User { get; init; }
    public bool Vote { get; init; }
    public string Body { get; set; }
}
