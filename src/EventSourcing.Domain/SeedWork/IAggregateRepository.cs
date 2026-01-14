namespace EventSourcing.Domain.Seedwork;

public interface IAggregateRepository<T> where T : AggregateRoot
{
    /// <summary>
    /// Get an aggregate by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The aggregate root generic.</returns>
    public Task<T?> LoadAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save an aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate root.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task.</returns>
    public Task SaveAsync(T aggregate, CancellationToken cancellationToken = default);

}
