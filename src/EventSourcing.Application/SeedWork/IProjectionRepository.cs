namespace EventSourcing.Application.SeedWork;

public interface IProjectionRepository<T> where T : notnull
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
}
