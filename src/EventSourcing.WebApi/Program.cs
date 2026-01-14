using EventSourcing.WebApi.Endpoints;
using EventSourcing.Infrastructure;
using EventSourcing.Application;
using EventSourcing.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddOpenApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
    _ = app.MapScalarConfig();
}

app.UseHttpsRedirection();

app.MapPartyEndpoints();
app.MapAccountEndpoints();

app.Run();
