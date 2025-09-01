namespace EventSourcing.Domain.Seedwork;

using System.Collections.Generic;

public abstract class AggregateRoot
{
    private readonly List<object> uncommittedEvents = [];

    public Guid Id { get; protected set; }
    public int Version { get; protected set; }

    public object[] GetUncommittedEvents()
        => [.. this.uncommittedEvents];

    public void MarkEventsAsCommitted()
        => this.uncommittedEvents.Clear();

    protected void RaiseEvent(object @event)
    {
        this.ApplyEvent(@event);
        this.uncommittedEvents.Add(@event);
    }

    protected void ApplyEvent(object @event)
    {
        // The Apply method convention uses dynamic dispatch to call the correct
        // private Apply method within the aggregate based on the event type.
        (this as dynamic).Apply(@event);
        this.Version++;
    }
}
