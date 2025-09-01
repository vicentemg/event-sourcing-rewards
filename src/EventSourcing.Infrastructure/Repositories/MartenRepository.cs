namespace EventSourcing.Infrastructure.Repositories;

using global::Marten;

using EventSourcing.Domain.Seedwork;

public class MartenRepository<T>(IDocumentSession session) : IRepository<T> where T : AggregateRoot, new()
{
    private readonly IDocumentSession session = session;

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.session.Events.AggregateStreamAsync<T>(id, token: cancellationToken);

    public Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.GetUncommittedEvents();
        if (uncommittedEvents.Length == 0)
        {
            return Task.CompletedTask;
        }

        var expectedVersion = aggregate.Version - uncommittedEvents.Length;

        if (expectedVersion == 0)
        {
            _ = this.session.Events.StartStream<T>(aggregate.Id, uncommittedEvents);
        }
        else
        {
            // Use the expected version for optimistic concurrency control.
            // This will throw a Marten.Exceptions.ConcurrencyException if the stream has been modified since the aggregate was loaded.
            _ = this.session.Events.Append(aggregate.Id, expectedVersion, uncommittedEvents);
        }

        // Important! The repository is also responsible for clearing the uncommitted events.
        aggregate.MarkEventsAsCommitted();

        return Task.CompletedTask;
    }

}
