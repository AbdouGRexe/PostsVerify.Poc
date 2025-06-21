using PostsVerify.Poc.Api.Application.Abstractions;
using PostsVerify.Poc.Api.Domain;
using PostsVerify.Poc.Api.Dtos;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

internal class AddReviewService : IAddReviewService
{
    private readonly PostsVerifyDbContext _context;

    public AddReviewService(PostsVerifyDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(AddReviewIDto input)
    {
        var review = new Review
        {
            PostId = input.PostId,
            UserId = input.UserId,
            Vote = input.Vote,
            Body = input.Body
        };

        await _context.AddAsync(review);

        if (await  _context.SaveChangesAsync() == 0)
        {
            throw new Exception();
        }

        return review.Id;
    }
}
