using System.Collections.Generic;

namespace PostsVerify.Poc.Api.Domain;

public class Source
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Link { get; set; }
    public byte? Score { get; set; }
    public ICollection<Post> Posts { get; set; }
}
