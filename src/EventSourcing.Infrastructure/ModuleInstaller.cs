namespace EventSourcing.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
public static class ModuleInstaller
{
    /// <summary>
    /// Registers infrastructure services for event sourcing.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your infrastructure services here.
        // Example:
        // services.AddSingleton<IEventStore, EventStore>();
        // services.AddScoped<IEventPublisher, EventPublisher>();
        services
            .RegisterMarten(configuration);

        return services;
    }
}
