namespace EventSourcing.Infrastructure.Marten.Configuration;

using global::Marten;
using System;
using System.Linq;


public static class ProjectionsExtensions
{
    public static StoreOptions AddMartenProjections(this StoreOptions options)
    {
        var configurationType = typeof(IProjectionConfiguration);
        var configurations = typeof(ProjectionsExtensions).Assembly
            .GetTypes()
            .Where(t => configurationType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IProjectionConfiguration)
            .Where(p => p != null)
            .ToList();

        foreach (var config in configurations)
        {
            config!.Configure(options);
        }

        return options;
    }
}
