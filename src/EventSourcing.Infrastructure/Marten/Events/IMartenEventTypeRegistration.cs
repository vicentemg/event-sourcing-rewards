namespace EventSourcing.Infrastructure.Marten.Events;

using global::Marten;

public interface IMartenEventTypeRegistration
{
    public void Register(StoreOptions options);
}
