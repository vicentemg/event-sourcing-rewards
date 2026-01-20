namespace EventSourcing.Infrastructure.Marten.Repositories;

using EventSourcing.Application.SeedWork;
using global::Marten;

public class ProjectionRepository<T>(IQuerySession session) : IProjectionRepository<T> where T : notnull
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await session.LoadAsync<T>(id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await session.Query<T>().ToListAsync(cancellationToken);
    }
}
