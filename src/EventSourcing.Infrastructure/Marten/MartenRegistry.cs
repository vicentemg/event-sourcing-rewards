namespace EventSourcing.Infrastructure.Marten;

using Domain.Seedwork;
using Repositories;
using global::Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class MartenRegistry
{
    public static IServiceCollection RegisterMarten(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(MartenRepository<>))
            .AddMarten(_ =>
            {
                var connectionString = configuration.GetConnectionString("postgres")!;

                _.Connection(connectionString);

                // this is the default behavior
                // opts.Projections.LiveStreamAggregation<Party>();

            }).UseLightweightSessions();

        return services;
    }
}
