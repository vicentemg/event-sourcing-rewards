namespace EventSourcing.Infrastructure.Marten.Events;

using EventSourcing.Infrastructure.Marten.Configuration;
using global::Marten;
using EventSourcing.Domain.Aggregates.PartyAggregate.Events;

public class PartyEventsConfiguration : IEventTypeConfiguration
{
    public void Configure(StoreOptions options)
    {
        _ = options.Events
                    .AddEventType<PartyCreated>();
    }
}
