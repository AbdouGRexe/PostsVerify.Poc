using System;
using System.Collections.Generic;

namespace PostsVerify.Poc.Api.Domain;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int? SourceId { get; set; }
    public Source Source { get; set; }
    public int? AuthorUserId { get; set; }
    public User AuthorUser { get; set; }
    public int? AreaId { get; set; }
    public Area Area { get; set; }
    public int CreatorUserId { get; set; }
    public User CreatorUser { get; set; }
    public DateTime DateCreation { get; set; }
    public byte? Score { get; set; }
    public DateTime? DateLastScoreCalculation { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
