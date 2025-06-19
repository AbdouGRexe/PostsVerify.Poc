using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PostsVerify.Poc.Api.Application.Abstractions;

namespace PostsVerify.Poc.Api.Endpoints;

public static class DataEndpoints
{
    public static RouteGroupBuilder RegisterDataEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/users",
            ([FromServices] IGetDataService service) =>
            service.GetUsersAsync())
            .WithName("get-users");

        app.MapGet("/sources",
            ([FromServices] IGetDataService service) =>
            service.GetSourcesAsync())
            .WithName("get-sources");

        app.MapGet("/areas",
            ([FromServices] IGetDataService service) =>
            service.GetAreasAsync())
            .WithName("get-areas");

        return app;
    }
}
