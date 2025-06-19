namespace PostsVerify.Poc.Api.Dtos;

public class AddPostIDto
{
    public string Title { get; init; }
    public string Body { get; init; }
    public string Source { get; init; }
    public string Link { get; init; }
    public string Author { get; init; }
    public string Area { get; init; }
    public string Creator { get; init; }
    public bool Verify { get; init; }
}
