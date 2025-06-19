using System.Text.Json.Serialization;

namespace PostsVerify.Poc.Api.Dtos;

public class AddReviewIDto
{
    public bool Vote { get; init; }
    public string Body { get; init; }
    public int UserId { get; set; }
    [JsonIgnore]
    public int PostId { get; private set; }
    public AddReviewIDto Alter(int postId) 
    { 
        PostId = postId; 
        return this; 
    }
}
