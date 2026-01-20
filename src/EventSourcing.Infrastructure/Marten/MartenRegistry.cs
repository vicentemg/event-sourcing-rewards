namespace EventSourcing.Infrastructure.Marten;

using Domain.Seedwork;
using global::Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventSourcing.Infrastructure.Marten.Configuration;
using EventSourcing.Infrastructure.Marten.Events;
using EventSourcing.Infrastructure.Marten.Repositories;
using JasperFx.Events.Daemon;
using global::Marten.Services;
using Weasel.Core;

public static class MartenRegistry
{
    public static IServiceCollection RegisterMarten(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres")!;

        _ = services.AddMarten(opts =>
        {
            opts.Connection(connectionString);

            _ = opts.AddMartenSerialization()
                .AddMartenEventTypes()
                .AddMartenProjections();
        })
        .AddAsyncDaemon(DaemonMode.Solo);

        return services;
    }
}
