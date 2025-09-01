namespace EventSourcing.WebApi.Endpoints;

using System.Threading.Tasks;
using EventSourcing.Domain.Aggregates.PartyAggregate;
using EventSourcing.Domain.Seedwork;
using Marten;
using Microsoft.AspNetCore.Mvc;

public static class PartyEndpoints
{
    public static void MapPartyEndpoints(this WebApplication app)
    {
        app
        .MapGet("/parties", GetPartiesAsync)
        .WithName("GetParties")
        .Produces<List<Party>>(StatusCodes.Status200OK);

        app
        .MapPost("/parties", CreatePartyAsync)
        .WithName("CreateParty")
        .Produces<Party>(StatusCodes.Status201Created);
    }

    internal static async Task<IResult> GetPartiesAsync([FromServices] IRepository<Party> repository)
    {
        var parties = await repository.GetAllAsync();
        return Results.Ok(parties);
    }

    internal static async Task<IResult> CreatePartyAsync(CreatePartyRequest create, [FromServices] IRepository<Party> repository, [FromServices] IDocumentSession session)
    {
        var result = Party.Create(create.Name, create.Email);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }

        var party = (Party)result;
        await repository.SaveAsync(party);
        await session.SaveChangesAsync();
        return Results.Created($"/parties/{party.Id}", party);
    }
}

public record CreatePartyRequest(string Name, string Email);
