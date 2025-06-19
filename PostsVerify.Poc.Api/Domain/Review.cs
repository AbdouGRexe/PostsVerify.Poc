namespace PostsVerify.Poc.Api.Domain;

public class Review
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public bool Vote { get; set; }
    public string Body { get; set; }
}