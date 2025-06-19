using Microsoft.EntityFrameworkCore;
using PostsVerify.Poc.Api.Application.Abstractions;
using PostsVerify.Poc.Api.Dtos;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

internal class GetPostService : IGetPostService
{
    private readonly PostsVerifyDbContext _context;

    public GetPostService(PostsVerifyDbContext context)
    {
        _context = context;
    }

    public Task<GetPostTDto> GetAsync(int postId)
    {
        return _context.Posts
            .Where(post => post.Id == postId)
            .Select(post => new GetPostTDto
            {
                Title = post.Title,
                Body = post.Body,
                SourceId = post.SourceId,
                Source = post.Source.Label,
                Link = post.Source.Link,
                AuthorUserId = post.AuthorUserId,
                Author = post.AuthorUser.Label,
                AreaId = post.AreaId,
                Area = post.Area.Label,
                DateCreation = post.DateCreation,
                Score = post.Score,
                DateLastScoreCalculation = post.DateLastScoreCalculation
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GetPostReviewsTDto> GetReviewsAsync(int postId)
    {
        var reviews = await _context.Reviews
            .Where(review => review.PostId == postId)
            .Select(review => new ReviewTDto
            {
                Id = review.Id,
                User = review.User.Label,
                Vote = review.Vote,
                Body = review.Body
            })
            .ToArrayAsync();

        var votesScore = 0;
        foreach (var review in reviews)
        {
            votesScore += review.Vote ? 1 : -1;
        }

        return new GetPostReviewsTDto
        {
            VotesScore = votesScore,
            Reviews = reviews
        };
    }

    public Task<GetPostReviewTDto> GetReviewAsync(int reviewId)
    {
        return _context.Reviews
            .Where(review => review.Id == reviewId)
            .Select(review => new GetPostReviewTDto
            {
                User = review.User.Label,
                Vote = review.Vote,
                Body = review.Body
            })
            .FirstOrDefaultAsync();
    }
}