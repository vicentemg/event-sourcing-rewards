using Marten;

namespace Eventsourcing.WebApi.Extensions;

internal static class EventSourcingExtensions
{
    public static IServiceCollection AddEventSourcing(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMarten(options => options.Connection(configuration.GetConnectionString("postgres")!))
            .UseLightweightSessions();


        return services;
    }
}