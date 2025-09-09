namespace EventSourcing.Application.Features.Party.Queries;

using EventSourcing.Domain.Aggregates.PartyAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record PartyDto(Guid Id, string Name, string Email);

public record GetPartyQuery(Guid PartyId);

public interface IGetPartyQueryHandler
{
    public Task<Result<PartyDto>> Handle(GetPartyQuery query, CancellationToken cancellationToken);
}

public class GetPartyQueryHandler(IRepository<Party> repository, ILogger<GetPartyQueryHandler> logger) : IGetPartyQueryHandler
{
    private static readonly Action<ILogger, Guid, Exception?> PartyNotFound =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(1, nameof(PartyNotFound)), "Party with ID {PartyId} not found.");

    private static readonly Action<ILogger, Guid, Exception?> HandlingGetPartyQuery =
        LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(2, nameof(HandlingGetPartyQuery)), "Handling GetPartyQuery for PartyId: {PartyId}");

    public async Task<Result<PartyDto>> Handle(GetPartyQuery query, CancellationToken cancellationToken)
    {
        HandlingGetPartyQuery(logger, query.PartyId, null);

        var party = await repository.GetAsync(query.PartyId, cancellationToken);
        if (party is null)
        {
            PartyNotFound(logger, query.PartyId, null);
            return Result.Fail<PartyDto>($"Party with ID {query.PartyId} not found.");
        }

        return Result.Ok(new PartyDto(party.Id, party.Name, party.Email));
    }
}
