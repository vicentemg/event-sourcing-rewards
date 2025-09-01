namespace EventSourcing.Application;

using Microsoft.Extensions.DependencyInjection;

public static class ModuleInstaller
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services;
}
