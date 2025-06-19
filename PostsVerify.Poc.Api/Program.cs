using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PostsVerify.Poc.Api.Application;
using PostsVerify.Poc.Api.Endpoints;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational;
using PostsVerify.Poc.Api.Infrastructure.Storage.Relational.Datasets;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

// 1 - Add services to the container.
builder.Services.AddModuleApplication();
builder.Services.AddModuleInfrastructureStorageRelational(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//SeedDatabase.Seed(app);

// 2 - Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();
app.Run();
