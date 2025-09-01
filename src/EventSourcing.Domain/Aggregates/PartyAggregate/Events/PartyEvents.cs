namespace EventSourcing.Domain.Aggregates.PartyAggregate.Events;

public record PartyCreated(Guid PartyId, string Name, string Email, DateTime OccurredOn);
