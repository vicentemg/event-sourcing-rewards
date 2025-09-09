namespace EventSourcing.Domain.Aggregates.PartyAggregate.Events;

public sealed record PartyCreated(Guid PartyId, string Name, string Email, DateTime OccurredOn);
