namespace EventSourcing.Infrastructure.Repositories;

using global::Marten;

using EventSourcing.Domain.Seedwork;

public class AggregateRepository<T>(IDocumentSession session) : IAggregateRepository<T> where T : AggregateRoot, new()
{
    public async Task<T?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var stream = await session.Events.FetchForWriting<T>(id, cancellationToken);
        return stream.Aggregate;
    }

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedEvents();

        if (uncommittedEvents.Length == 0)
        {
            return;
        }

        if (aggregate.Version == 0)
        {
            session.Events.StartStream<T>(aggregate.Id, uncommittedEvents);
        }
        else
        {
            await session.Events.AppendOptimistic(aggregate.Id, aggregate.Version, uncommittedEvents);
        }

        await session.SaveChangesAsync(cancellationToken);
        aggregate.MarkEventsAsCommitted();
    }
}
