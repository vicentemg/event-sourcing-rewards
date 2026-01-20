namespace EventSourcing.Infrastructure.Marten.Events;

using EventSourcing.Infrastructure.Marten.Configuration;
using EventSourcing.Domain.Aggregates.AccountAggregate.Events;
using global::Marten;

public class AccountEventsConfiguration : IEventTypeConfiguration
{
    public void Configure(StoreOptions options)
    {
        _ = options.Events
            .AddEventType<AccountCreated>()
            .AddEventType<FundsDeposited>()
            .AddEventType<FundsWithdrawn>()
            .AddEventType<DebtIncurred>();
    }
}
