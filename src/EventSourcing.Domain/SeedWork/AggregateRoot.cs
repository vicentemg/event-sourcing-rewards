namespace EventSourcing.Domain.Seedwork;

using System.Collections.Generic;

public interface IAggregateRoot
{
    // Marker interface, no members required
}

public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<object> _uncommittedEvents = [];

    public Guid Id { get; protected set; }

    public long Version { get; set; }

    public object[] GetUncommittedEvents()
    {
        return [.. _uncommittedEvents];
    }

    public void MarkEventsAsCommitted()
    {
        _uncommittedEvents.Clear();
    }

    protected void RaiseEvent(object @event)
    {
        ApplyEvent(@event);
        _uncommittedEvents.Add(@event);
    }

    protected void ApplyEvent(object @event)
    {
        (this as dynamic).Apply((dynamic)@event);
        Version++;
    }
}
