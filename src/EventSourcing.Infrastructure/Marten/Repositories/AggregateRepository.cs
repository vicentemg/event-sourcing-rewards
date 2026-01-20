namespace EventSourcing.Infrastructure.Marten.Repositories;

using global::Marten;

using EventSourcing.Domain.Seedwork;

public class AggregateRepository<T>(IDocumentSession session) : IAggregateRepository<T> where T : AggregateRoot
{
    public async Task<T?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await session.Events.FetchLatest<T>(id, cancellationToken);
    }

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedEvents();

        if (uncommittedEvents.Length == 0)
        {
            return;
        }

        // Since the aggregate has already applied the events, its version has been incremented.
        // We need to get the version BEFORE these events were applied for optimistic concurrency check.
        var versionBeforeEvents = aggregate.Version - uncommittedEvents.Length;

        if (versionBeforeEvents == 0)
        {
            // This is a new aggregate, no prior events
            _ = session.Events.StartStream<T>(aggregate.Id, uncommittedEvents);
        }
        else
        {
            // Explicitly opt into optimistic concurrency checks by telling Marten the expected starting version
            var stream = await session.Events.FetchForWriting<T>(aggregate.Id, versionBeforeEvents, cancellationToken);
            stream.AppendMany(uncommittedEvents);
        }

        await session.SaveChangesAsync(cancellationToken);

        aggregate.MarkEventsAsCommitted();
    }
}
