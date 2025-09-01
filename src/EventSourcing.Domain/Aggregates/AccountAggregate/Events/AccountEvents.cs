namespace EventSourcing.Domain.Aggregates.AccountAggregate.Events;

public record AccountCreated(Guid AccountId, Guid PartyId, DateTime OccurredOn);

public record FundsDeposited(Guid AccountId, decimal Amount, DateTime OccurredOn);

public record FundsWithdrawn(Guid AccountId, decimal Amount, DateTime OccurredOn);
