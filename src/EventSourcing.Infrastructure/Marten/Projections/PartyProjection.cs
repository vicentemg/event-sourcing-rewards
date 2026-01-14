namespace EventSourcing.Infrastructure.Marten.Projections;

using global::Marten;
using EventSourcing.Domain.Aggregates.PartyAggregate;

public class PartyProjection : IMartenProjection
{
    public void Configure(StoreOptions options)
    {
        _ = options.Projections.LiveStreamAggregation<Party>();
    }
}
