namespace EventSourcing.Infrastructure.Marten.Events;

using global::Marten;
using System;
using System.Linq;

public static class EventTypesExtensions
{
    public static StoreOptions AddMartenEventTypes(this StoreOptions options)
    {
        var registrationType = typeof(IMartenEventTypeRegistration);
        var registrations = typeof(EventTypesExtensions).Assembly
            .GetTypes()
            .Where(t => registrationType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IMartenEventTypeRegistration)
            .Where(r => r != null);

        foreach (var registration in registrations)
        {
            registration!.Register(options);
        }

        return options;
    }
}
