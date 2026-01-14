namespace EventSourcing.Application.Features.Account.Queries.GetAccount;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Aggregates.AccountAggregate.Events;

/// <summary>
/// Read model for Account. This document is maintained by the AccountProjection
/// and updated when Account events are emitted.
/// This represents the query-side view of an Account.
/// </summary>
public class GetAccountModel
{
    public Guid Id { get; set; }
    public Guid PartyId { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }

    public GetAccountModel() { }

    public GetAccountModel(Guid id, Guid partyId, decimal balance, AccountStatus status)
    {
        Id = id;
        PartyId = partyId;
        Balance = balance;
        Status = status;
    }

    /// <summary>
    /// Apply AccountCreated event to initialize the read model.
    /// </summary>
    public void Apply(AccountCreated @event)
    {
        Id = @event.AccountId;
        PartyId = @event.PartyId;
        Balance = 0m;
        Status = AccountStatus.Active;
    }

    /// <summary>
    /// Apply FundsDeposited event to increment the balance.
    /// </summary>
    public void Apply(FundsDeposited @event)
    {
        Balance += @event.Amount.Amount;
    }

    /// <summary>
    /// Apply FundsWithdrawn event to decrement the balance.
    /// </summary>
    public void Apply(FundsWithdrawn @event)
    {
        Balance -= @event.Amount.Amount;
    }

    /// <summary>
    /// Apply DebtIncurred event to decrement the balance.
    /// This event represents a liability/debt incurred by the account.
    /// </summary>
    public void Apply(DebtIncurred @event)
    {
        Balance -= @event.Amount.Amount;
    }
}
