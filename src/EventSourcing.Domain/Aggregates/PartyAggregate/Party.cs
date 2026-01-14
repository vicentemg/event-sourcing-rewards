namespace EventSourcing.Domain.Aggregates.PartyAggregate;

using EventSourcing.Domain.Aggregates.PartyAggregate.Events;
using EventSourcing.Domain.Seedwork;

/// <summary>
/// Represents a customer or entity that can own accounts.
/// </summary>
public class Party : AggregateRoot
{
    public Party() { }
    public Party(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public static Result<Party> Create(Guid id, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<Party>("Party name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Fail<Party>("Party email cannot be empty.");
        }

        var party = new Party(id, name, email);

        party.RaiseEvent(new PartyCreated(Guid.NewGuid(), name, email, DateTime.UtcNow));

        return Result.Ok(party);
    }

    public void Apply(PartyCreated e)
    {
        Id = e.PartyId;
        Name = e.Name;
        Email = e.Email;
    }
}
