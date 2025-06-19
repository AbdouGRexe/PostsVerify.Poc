using Microsoft.Extensions.DependencyInjection;
using PostsVerify.Poc.Api.Application.Abstractions;

namespace PostsVerify.Poc.Api.Application;

public static class ModuleApplication
{
    public static IServiceCollection AddModuleApplication(this IServiceCollection services)
    {
        services.AddScoped<IAddPostService, AddPostService>();
        services.AddScoped<IAddReviewService, AddReviewService>();
        services.AddScoped<IVerifyPostService, VerifyPostService>();
        services.AddScoped<IGetPostService, GetPostService>();
        services.AddScoped<IGetDataService, GetDataService>();

        return services;
    }
}
