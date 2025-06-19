using System;

namespace PostsVerify.Poc.Api.Dtos;

public class GetPostTDto
{
    public string Title { get; init; }
    public string Body { get; init; }
    public int? SourceId { get; init; }
    public string Source { get; init; }
    public string Link { get; init; }
    public int? AuthorUserId { get; init; }
    public string Author { get; init; }
    public int? AreaId { get; init; }
    public string Area { get; init; }
    public DateTime DateCreation { get; init; }
    public byte? Score { get; init; }
    public DateTime? DateLastScoreCalculation { get; init; }
}
