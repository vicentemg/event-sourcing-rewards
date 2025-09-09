namespace EventSourcing.Infrastructure.Marten.Projections;

using global::Marten;
using System;
using System.Linq;

public static class ProjectionsExtensions
{
    public static StoreOptions AddMartenProjections(this StoreOptions options)
    {
        var projectionType = typeof(IMartenProjection);
        var projections = typeof(ProjectionsExtensions).Assembly
            .GetTypes()
            .Where(t => projectionType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IMartenProjection)
            .Where(p => p != null);

        foreach (var projection in projections)
        {
            projection!.Configure(options);
        }

        return options;
    }
}
