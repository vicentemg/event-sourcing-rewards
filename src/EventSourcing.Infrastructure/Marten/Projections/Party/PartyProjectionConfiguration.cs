namespace EventSourcing.Infrastructure.Marten.Projections.Party;

using EventSourcing.Domain.Aggregates.PartyAggregate;
using EventSourcing.Infrastructure.Marten.Configuration;
using global::Marten;

public class PartyProjectionConfiguration : IProjectionConfiguration
{
    public void Configure(StoreOptions options)
    {
        _ = options.Projections.LiveStreamAggregation<Party>();
    }
}
