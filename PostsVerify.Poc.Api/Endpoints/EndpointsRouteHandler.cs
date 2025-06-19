using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PostsVerify.Poc.Api.Endpoints;

public static class EndpointsRouteHandler
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("");

        api.MapGroup("/posts").RegisterPostsEndpoints().WithTags("Posts");

        api.MapGroup("/data").RegisterDataEndpoints().WithTags("Data");

        return app;
    }
}
