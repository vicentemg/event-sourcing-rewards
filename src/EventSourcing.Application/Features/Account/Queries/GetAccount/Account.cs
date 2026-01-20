namespace EventSourcing.Application.Features.Account.Queries.GetAccount;

using EventSourcing.Domain.Aggregates.AccountAggregate;

/// <summary>
/// Read model for Account. This document is maintained by the AccountProjection
/// and updated when Account events are emitted.
/// This represents the query-side view of an Account.
/// </summary>
public class Account
{
    public Guid Id { get; set; }
    public Guid PartyId { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public int Version { get; set; }

    public Account() { }

    public Account(Guid id, Guid partyId, decimal balance, AccountStatus status)
    {
        Id = id;
        PartyId = partyId;
        Balance = balance;
        Status = status;
    }

}
