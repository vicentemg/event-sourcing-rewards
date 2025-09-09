namespace EventSourcing.Domain.Aggregates.AccountAggregate.Events;

public sealed record AccountCreated(Guid AccountId, Guid PartyId, DateTime OccurredOn);

public sealed record FundsDeposited(Guid AccountId, decimal Amount, DateTime OccurredOn);

public sealed record FundsWithdrawn(Guid AccountId, decimal Amount, DateTime OccurredOn);
