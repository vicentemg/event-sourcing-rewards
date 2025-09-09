namespace EventSourcing.Application;

using Microsoft.Extensions.DependencyInjection;

public static class ModuleInstaller
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ModuleInstaller).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if ((iface.Name.EndsWith("QueryHandler", StringComparison.Ordinal) || iface.Name.EndsWith("CommandHandler", StringComparison.Ordinal)) && type.IsClass && !type.IsAbstract)
                {
                    _ = services.AddScoped(iface, type);
                }
            }
        }
        return services;
    }
}
