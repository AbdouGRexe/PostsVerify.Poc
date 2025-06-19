using System.Collections.Generic;

namespace PostsVerify.Poc.Api.Domain;

public class User
{
    public int Id { get; set; }
    public string Label { get; set; }
    public int? AreaId { get; set; }
    public Area Area { get; set; }
    public byte? Score { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Post> CreatedPosts { get; set; }
    public ICollection<Post> AuthoredPosts { get; set; }
}
