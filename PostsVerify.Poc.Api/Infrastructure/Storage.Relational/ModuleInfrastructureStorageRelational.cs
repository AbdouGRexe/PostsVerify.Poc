using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.Datasets;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;
using System;
using System.IO;

namespace PostsVerify.Poc.Api.Infrastructure.Storage.Relational;

public static class ModuleInfrastructureStorageRelational
{
    public static IServiceCollection AddModuleInfrastructureStorageRelational(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkCore(configuration);
        //services.AddTransient<SeedDatabase>();

        return services;
    }

    public static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostsVerifyDbContext>(options =>
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
            var connectionString = configuration
                .GetConnectionString("PostsVerifyRelationalDatabase")
                .Replace("{PATH}", path);

            options.UseSqlite(connectionString);
        });

        return services;
    }
}
