namespace EventSourcing.Domain.Aggregates.AccountAggregate.Events;

using EventSourcing.Domain.Seedwork;

public sealed record AccountCreated(Guid AccountId, Guid PartyId, DateTime OccurredOn);

public sealed record FundsDeposited(Guid AccountId, Money Amount, Merchant Merchant, DateTime OccurredOn);

public sealed record FundsWithdrawn(Guid AccountId, Money Amount, Merchant Merchant, DateTime OccurredOn);

public sealed record DebtIncurred(Guid AccountId, Money Amount, Merchant Merchant, DateTime OccurredOn);
