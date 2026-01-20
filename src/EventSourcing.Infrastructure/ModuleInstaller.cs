namespace EventSourcing.Infrastructure;

using EventSourcing.Infrastructure.Marten;
using EventSourcing.Application.SeedWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using global::Marten.Exceptions;
using EventSourcing.Infrastructure.Behaviors;
using Polly;
using System;
using EventSourcing.Infrastructure.Marten.Repositories;
using EventSourcing.Domain.Seedwork;

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
        _ = services
            .RegisterMarten(configuration)
            .AddScoped(typeof(IAggregateRepository<>), typeof(AggregateRepository<>))
            .AddScoped(typeof(IProjectionRepository<>), typeof(ProjectionRepository<>));

        _ = services.AddSingleton<AsyncPolicy>(Policy
            .Handle<ConcurrentUpdateException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt))));

        _ = services.Decorate(typeof(ICommandHandler<,>), typeof(ConcurrencyRetryDecorator<,>));

        return services;
    }
}
