namespace EventSourcing.Infrastructure.Marten.Projections;

using global::Marten;
using EventSourcing.Domain.Aggregates.PartyAggregate;

public class PartyProjection : IMartenProjection
{
    public void Configure(StoreOptions options)
    {
        options.Projections.LiveStreamAggregation<Party>();
    }
}
