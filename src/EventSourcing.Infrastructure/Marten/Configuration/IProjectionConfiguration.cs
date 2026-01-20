namespace EventSourcing.Infrastructure.Marten.Configuration;

using global::Marten;

public interface IProjectionConfiguration
{
    void Configure(StoreOptions options);
}
