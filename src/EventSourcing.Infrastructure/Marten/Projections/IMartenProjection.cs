namespace EventSourcing.Infrastructure.Marten.Projections;

using global::Marten;

public interface IMartenProjection
{
    public void Configure(StoreOptions options);
}
