using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostsVerify.Poc.Api.Domain;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PostsVerify.Poc.Api.Infrastructure.Storage.Relational.Datasets;

public class SeedDatabase
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<PostsVerifyDbContext>();
            context.Database.Migrate();
            SeedAreas(context);
            SeedUsers(context);
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, $"Error : {nameof(SeedDatabase)}");
        }
    }

    private static void SeedUsers(PostsVerifyDbContext context)
    {
        if (!context.Users.Any())
        {
            var areaData = System.IO.File.ReadAllText("Infrastructure/Storage.Relational/SeedDatabase/Data/AreaSeedData.json");
            var areas = JsonSerializer.Deserialize<List<Area>>(areaData);
            foreach (var area in areas)
            {
                context.Areas.Add(area);
            }
            context.SaveChanges();

            var userData = System.IO.File.ReadAllText("Infrastructure/Storage.Relational/SeedDatabase/Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<User>>(userData);
            var random = new Random();
            foreach (var user in users)
            {
                user.AreaId = random.Next(1, 3);
                user.Score = (byte)random.Next(1, 10);
            }
            context.SaveChanges();
        }
    }

    private static void SeedAreas(PostsVerifyDbContext context)
    {
        if (!context.Areas.Any())
        {
            var areaData = System.IO.File.ReadAllText("Infrastructure/Storage.Relational/SeedDatabase/Data/AreaSeedData.json");
            var areas = JsonSerializer.Deserialize<List<Area>>(areaData);
            foreach (var area in areas)
            {
                context.Areas.Add(area);
            }
            context.SaveChanges();
        }
    }
}