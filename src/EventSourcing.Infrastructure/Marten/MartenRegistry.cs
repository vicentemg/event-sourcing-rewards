namespace EventSourcing.Infrastructure.Marten;

using Domain.Seedwork;
using Repositories;
using global::Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventSourcing.Infrastructure.Marten.Projections;
using EventSourcing.Infrastructure.Marten.Events;

public static class MartenRegistry
{
    public static IServiceCollection RegisterMarten(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(MartenRepository<>))
            .AddMarten(opts =>
            {
                var connectionString = configuration.GetConnectionString("postgres")!;
                opts.Connection(connectionString);

                _ = opts
                    .AddMartenEventTypes()
                    .AddMartenProjections();

            }).UseLightweightSessions();

        return services;
    }
}
