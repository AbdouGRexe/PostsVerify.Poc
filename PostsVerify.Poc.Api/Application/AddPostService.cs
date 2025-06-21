using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostsVerify.Poc.Api.Domain;
using PostsVerify.Poc.Api.Dtos;
using PostsVerify.Poc.Api.Application.Abstractions;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;

namespace PostsVerify.Poc.Api.Application;

internal class AddPostService : IAddPostService
{
    private readonly PostsVerifyDbContext _context;

    public AddPostService(PostsVerifyDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(AddPostIDto input)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var post = new Post
            {
                Title = input.Title,
                Body = input.Body,
                DateCreation = DateTime.Now
            };

            post.SourceId = await CreateSourceIf(input);
            post.AuthorUserId = await CreateAuthorIf(input);
            post.CreatorUserId = await CreateCreatorIf(input);
            post.AreaId = await CreateAreaIf(input);

            if (input.Verify)
            {
                post.Score = (byte)new Random().Next(1, 10);
                post.DateLastScoreCalculation = DateTime.Now;
            }

            await _context.AddAsync(post);

            if (await _context.SaveChangesAsync() == 0)
            {
                throw new Exception();
            }

            await transaction.CommitAsync();

            return post.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<int> CreateSourceIf(AddPostIDto input)
    {
        var source = await _context.Sources.FirstOrDefaultAsync(source => source.Label == input.Source);
        if (source != null)
        {
            return source.Id;
        }

        source = new Source { Label = input.Source, Link = input.Link, Score = (byte)new Random().Next(1, 10) };

        await _context.AddAsync(source);

        if (await _context.SaveChangesAsync() == 0)
        {
            throw new Exception();
        }

        return source.Id;
    }

    private async Task<int> CreateAuthorIf(AddPostIDto input)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Label == input.Author);
        if (user != null)
        {
            return user.Id;
        }

        user = new User { Label = input.Author, Score = (byte)new Random().Next(1, 10) };

        await _context.AddAsync(user);

        if (await _context.SaveChangesAsync() == 0)
        {
            throw new Exception();
        }

        return user.Id;
    }

    private async Task<int> CreateCreatorIf(AddPostIDto input)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Label == input.Creator);
        if (user != null)
        {
            return user.Id;
        }

        user = new User { Label = input.Creator, Score = (byte)new Random().Next(1, 10) };

        await _context.AddAsync(user);

        if (await _context.SaveChangesAsync() == 0)
        {
            throw new Exception();
        }

        return user.Id;
    }

    private async Task<int> CreateAreaIf(AddPostIDto input)
    {
        var area = await _context.Areas.FirstOrDefaultAsync(area => area.Label == input.Area);
        if (area != null)
        {
            return area.Id;
        }

        area = new Area { Label = input.Area };

        await _context.AddAsync(area);

        if (await _context.SaveChangesAsync() == 0)
        {
            throw new Exception();
        }

        return area.Id;
    }
}
