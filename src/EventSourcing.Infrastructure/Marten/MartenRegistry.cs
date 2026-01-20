namespace EventSourcing.Infrastructure.Marten;

using Domain.Seedwork;
using global::Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventSourcing.Infrastructure.Marten.Configuration;
using EventSourcing.Infrastructure.Marten.Events;
using EventSourcing.Infrastructure.Marten.Repositories;
using JasperFx.Events.Daemon;

public static class MartenRegistry
{
    public static IServiceCollection RegisterMarten(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services
            .AddMarten(opts =>
            {
                var connectionString = configuration.GetConnectionString("postgres")!;
                opts.Connection(connectionString);

                _ = opts
                    .AddMartenEventTypes()
                    .AddMartenProjections();

            })
            .AddAsyncDaemon(DaemonMode.Solo);

        return services;
    }
}
