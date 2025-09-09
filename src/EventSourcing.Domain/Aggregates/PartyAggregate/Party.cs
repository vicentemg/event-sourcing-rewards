namespace EventSourcing.Domain.Aggregates.PartyAggregate;

using EventSourcing.Domain.Aggregates.PartyAggregate.Events;
using EventSourcing.Domain.Seedwork;

/// <summary>
/// Represents a customer or entity that can own accounts.
/// </summary>
public class Party : AggregateRoot
{
    public Party() { }
    public Party(Guid id) => this.Id = id;
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public static Result<Party> Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<Party>("Party name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Fail<Party>("Party email cannot be empty.");
        }

        var party = new Party(Guid.NewGuid());
        party.RaiseEvent(new PartyCreated(Guid.NewGuid(), name, email, DateTime.UtcNow));
        return Result.Ok(party);
    }

    public void Apply(PartyCreated e)
    {
        this.Id = e.PartyId;
        this.Name = e.Name;
        this.Email = e.Email;
    }
}
