namespace EventSourcing.Infrastructure.Marten.Configuration;

using global::Marten;

public interface IEventTypeConfiguration
{
    public void Configure(StoreOptions options);
}
