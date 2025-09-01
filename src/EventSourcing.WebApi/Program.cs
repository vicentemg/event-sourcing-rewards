using EventSourcing.WebApi.Endpoints;
using EventSourcing.Infrastructure;
using EventSourcing.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPartyEndpoints();

app.Run();
