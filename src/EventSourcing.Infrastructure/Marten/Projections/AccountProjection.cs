namespace EventSourcing.Infrastructure.Marten.Projections;

using global::Marten;
using JasperFx.Events.Projections;
using global::Marten.Events.Aggregation;
using EventSourcing.Application.Features.Account.Queries.GetAccount;

/// <summary>
/// Inline projection that maintains the AccountReadModel document.
/// This projection is updated synchronously when Account events are committed,
/// ensuring strong consistency for read queries.
///
/// The AccountReadModel is treated as a live stream aggregation, meaning
/// it's reconstructed from events on each read (if not cached).
/// </summary>
public class AccountProjection : IMartenProjection
{
    public void Configure(StoreOptions options)
    {
        // Configure inline projection using live stream aggregation.
        // This will apply events from the Account stream to build/update
        // the AccountReadModel document with the same ID as the stream.
        options.Projections.Add(new SingleStreamProjection<GetAccountModel, Guid>(), ProjectionLifecycle.Inline);
    }
}
