namespace EventSourcing.Infrastructure.Marten.Events;

using global::Marten;
using EventSourcing.Domain.Aggregates.AccountAggregate.Events;

public class AccountEventsRegistration : IMartenEventTypeRegistration
{
    public void Register(StoreOptions options)
        => _ = options.Events
                .AddEventType<AccountCreated>()
                .AddEventType<FundsDeposited>()
                .AddEventType<FundsWithdrawn>();
}
