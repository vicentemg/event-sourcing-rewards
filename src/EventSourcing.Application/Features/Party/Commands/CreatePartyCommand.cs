namespace EventSourcing.Application.Features.Party.Commands;

using EventSourcing.Domain.Seedwork;
using EventSourcing.Domain.Aggregates.PartyAggregate;
using System;
using Microsoft.Extensions.Logging;
using EventSourcing.Application.SeedWork;

public record CreatePartyCommand(string Name, string Email);

public interface ICreatePartyCommandHandler : ICommandHandler<CreatePartyCommand, Guid>
{
}

public class CreatePartyCommandHandler(IAggregateRepository<Party> repository,
                                       ILogger<CreatePartyCommandHandler> logger) : ICreatePartyCommandHandler
{
    private static readonly Action<ILogger, string, string, Exception?> LogHandlingCreatePartyCommand =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling CreatePartyCommand for Name: {Name}, Email: {Email}");

    private static readonly Action<ILogger, string, Exception?> LogCreatePartyError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2, "CreatePartyError"),
            "Failed to create party: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogPartyCreated =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3, "PartyCreated"),
            "Party created with ID: {PartyId}");

    public async Task<Result<Guid>> Handle(CreatePartyCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingCreatePartyCommand(logger, command.Name, command.Email, null);

        var result = Party.Create(Guid.NewGuid(), command.Name, command.Email);

        if (result.IsFailure)
        {
            LogCreatePartyError(logger, result.Error, null);
            return Result.Fail<Guid>(result.Error);
        }

        var party = result.Value;
        await repository.SaveAsync(party, cancellationToken);

        LogPartyCreated(logger, party.Id, null);

        return Result.Ok(party.Id);
    }
}
