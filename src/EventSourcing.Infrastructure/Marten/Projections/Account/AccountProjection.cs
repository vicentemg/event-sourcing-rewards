namespace EventSourcing.Infrastructure.Marten.Projections.Account;

using EventSourcing.Domain.Aggregates.AccountAggregate.Events;
using AccountStatus = Domain.Aggregates.AccountAggregate.AccountStatus;
using JasperFx.Events;
using Application.Features.Account.Queries.GetAccount;
public class AccountProjection : SingleStreamProjection<Account, Guid>
{
    public Account Create(IEvent<AccountCreated> @event)
    {
        return new Account(@event.Data.AccountId, @event.Data.PartyId, 0, AccountStatus.Active);
    }

    public void Apply(IEvent<FundsDeposited> @event, Account account)
    {
        account.Balance += @event.Data.Amount.Amount;
    }

    public void Apply(IEvent<FundsWithdrawn> @event, Account account)
    {
        account.Balance -= @event.Data.Amount.Amount;
    }

    public void Apply(IEvent<DebtIncurred> @event, Account account)
    {
        account.Balance -= @event.Data.Amount.Amount;
    }
}
