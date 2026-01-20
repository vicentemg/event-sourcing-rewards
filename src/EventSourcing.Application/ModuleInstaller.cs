namespace EventSourcing.Application;

using Microsoft.Extensions.DependencyInjection;
using EventSourcing.Application.SeedWork;

public static class ModuleInstaller
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ModuleInstaller).Assembly;

        _ = services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        _ = services.Scan(scan => scan
             .FromAssemblies(assembly)
             .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
             .AsImplementedInterfaces()
             .WithScopedLifetime());

        return services;
    }
}
