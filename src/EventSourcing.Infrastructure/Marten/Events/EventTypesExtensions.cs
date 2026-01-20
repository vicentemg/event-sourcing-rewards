namespace EventSourcing.Infrastructure.Marten.Events;

using global::Marten;
using System;
using System.Linq;

using EventSourcing.Infrastructure.Marten.Configuration;

public static class EventTypesExtensions
{
    public static StoreOptions AddMartenEventTypes(this StoreOptions options)
    {
        var configurationType = typeof(IEventTypeConfiguration);
        var configurations = typeof(EventTypesExtensions).Assembly
            .GetTypes()
            .Where(t => configurationType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IEventTypeConfiguration)
            .Where(r => r != null);

        foreach (var config in configurations)
        {
            config!.Configure(options);
        }

        return options;
    }
}
