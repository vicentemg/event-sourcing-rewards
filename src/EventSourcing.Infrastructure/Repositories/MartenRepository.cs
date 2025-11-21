namespace EventSourcing.Infrastructure.Repositories;

using global::Marten;

using EventSourcing.Domain.Seedwork;

public class MartenRepository<T>(IDocumentSession session) : IRepository<T> where T : AggregateRoot, new()
{
    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await session.Events.AggregateStreamAsync<T>(id, token: cancellationToken);

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedEvents();
        if (uncommittedEvents.Length == 0)
        {
            return;
        }

        var expectedVersion = aggregate.Version - uncommittedEvents.Length;

        if (expectedVersion == 0)
        {
            session.Events.StartStream<T>(aggregate.Id, uncommittedEvents);
        }
        else
        {
            // Use the expected version for optimistic concurrency control.
            // This will throw a Marten.Exceptions.ConcurrencyException if the stream has been modified since the aggregate was loaded.
            session.Events.Append(aggregate.Id, expectedVersion, uncommittedEvents);
        }

        // Important! The repository is also responsible for clearing the uncommitted events.
        aggregate.MarkEventsAsCommitted();

        await session.SaveChangesAsync(cancellationToken);
    }
}
