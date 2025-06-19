using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PostsVerify.Poc.Api.Dtos;
using PostsVerify.Poc.Api.Application.Abstractions;

namespace PostsVerify.Poc.Api.Endpoints;

public static class PostsEndpoints
{
    public static RouteGroupBuilder RegisterPostsEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("",
            ([FromServices] IAddPostService service, AddPostIDto input) =>
            service.AddAsync(input))
            .WithName("add-post");

        app.MapPost("/{postId}/review",
            ([FromServices] IAddReviewService service, int postId, AddReviewIDto input) => 
            service.AddAsync(input.Alter(postId)))
            .WithName("add-review");

        app.MapPatch("/{postId}/verify",
            ([FromServices] IVerifyPostService service, int postId) => 
            service.VerifyAsync(postId))
            .WithName("verify-post");

        app.MapGet("/{postId}",
            ([FromServices] IGetPostService service, int postId) =>
            service.GetAsync(postId))
            .WithName("get-post");

        app.MapGet("/{postId}/reviews",
            ([FromServices] IGetPostService service, int postId) =>
            service.GetReviewsAsync(postId))
            .WithName("get-post-reviews");

        app.MapGet("/reviews/{reviewId}",
            ([FromServices] IGetPostService service, int reviewId) =>
            service.GetReviewAsync(reviewId))
            .WithName("get-post-review");

        return app;
    }
}
