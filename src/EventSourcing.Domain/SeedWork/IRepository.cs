namespace EventSourcing.Domain.Seedwork;

public interface IRepository<T> where T : AggregateRoot
{
    /// <summary>
    /// Get an aggregate by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save an aggregate.
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SaveAsync(T aggregate, CancellationToken cancellationToken = default);

}
