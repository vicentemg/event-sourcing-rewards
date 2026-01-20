namespace EventSourcing.WebApi.Endpoints;

using System.Threading.Tasks;
using EventSourcing.Application.Features.Party.Commands;
using EventSourcing.Application.Features.Party.Queries;
using EventSourcing.Domain.Aggregates.PartyAggregate;
using Microsoft.AspNetCore.Mvc;

public static class PartyEndpoints
{
    public static void MapPartyEndpoints(this WebApplication app)
    {
        _ = app
        .MapGet("/parties/{id}", GetPartyByIdAsync)
        .WithName("GetParty")
        .Produces<List<Party>>(StatusCodes.Status200OK);

        _ = app
        .MapPost("/parties", CreatePartyAsync)
        .WithName("CreateParty")
        .Produces<Party>(StatusCodes.Status201Created);
    }

    internal static async Task<IResult> GetPartyByIdAsync(Guid id, [FromServices] IGetPartyQueryHandler handler, CancellationToken cancellationToken)
    {
        var parties = await handler.Handle(new GetPartyQuery(id), cancellationToken);

        if (parties.IsFailure)
        {
            return Results.NotFound(parties.Error);
        }

        return Results.Ok(parties.Value);
    }

    internal static async Task<IResult> CreatePartyAsync(CreatePartyCommand command, [FromServices] ICreatePartyCommandHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }
        var partyId = result.Value;
        return Results.Created($"/parties/{partyId}", new { Id = partyId });
    }
}

