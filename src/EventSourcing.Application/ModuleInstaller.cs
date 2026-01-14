namespace EventSourcing.Application;

using Microsoft.Extensions.DependencyInjection;
using Polly;
using EventSourcing.Application.SeedWork;
using EventSourcing.Application.Behaviors;
using Marten.Exceptions;

public static class ModuleInstaller
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ModuleInstaller).Assembly;

        _ = services.AddSingleton(Policy
            .Handle<ConcurrentUpdateException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt))));

        _ = services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        _ = services.Decorate(typeof(ICommandHandler<,>), typeof(ConcurrencyRetryDecorator<,>));

        // Keep existing query handler registration for now if not refactored
        foreach (var type in assembly.GetTypes())
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.Name.EndsWith("QueryHandler", StringComparison.Ordinal) && type.IsClass && !type.IsAbstract)
                {
                    _ = services.AddScoped(iface, type);
                }
            }
        }

        return services;
    }
}
