namespace EventSourcing.Infrastructure.Marten.Events;

using global::Marten;
using EventSourcing.Domain.Aggregates.PartyAggregate.Events;

public class PartyEventsRegistration : IMartenEventTypeRegistration
{
    public void Register(StoreOptions options)
    {
        _ = options.Events
                    .AddEventType<PartyCreated>();
    }
}
