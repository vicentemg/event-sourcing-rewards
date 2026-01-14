namespace EventSourcing.Application.Features.Account.Commands.CreateAccount;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using EventSourcing.Application.SeedWork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Party = Domain.Aggregates.PartyAggregate.Party;

public record CreateAccountCommand(Guid Id, Guid PartyId, decimal InitialBalance);

public class CreateAccountCommandHandler(
    IAggregateRepository<Account> repository,
    IAggregateRepository<Party> partyRepository,
    ILogger<CreateAccountCommandHandler> logger) : ICommandHandler<CreateAccountCommand, Guid>
{
    private static readonly Action<ILogger, Guid, Guid, decimal, Exception?> LogHandlingCreateAccountCommand =
        LoggerMessage.Define<Guid, Guid, decimal>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling CreateAccountCommand for AccountId: {AccountId}, PartyId: {PartyId}, InitialBalance: {InitialBalance}");

    private static readonly Action<ILogger, string, Exception?> LogCreateAccountError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2, "CreateAccountError"),
            "Failed to create account: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogPartyNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(3, "PartyNotFound"),
            "Party with ID: {PartyId} not found");

    private static readonly Action<ILogger, Guid, Exception?> LogAccountCreated =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(4, "AccountCreated"),
            "Account created with ID: {AccountId}");

    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingCreateAccountCommand(logger, command.Id, command.PartyId, command.InitialBalance, null);

        var party = await partyRepository.LoadAsync(command.PartyId, cancellationToken);
        if (party is null)
        {
            LogPartyNotFound(logger, command.PartyId, null);
            return Result.Fail<Guid>("Party with ID " + command.PartyId + " not found.");
        }

        var initialBalanceResult = Money.Create(command.InitialBalance);
        if (initialBalanceResult.IsFailure)
        {
            LogCreateAccountError(logger, initialBalanceResult.Error, null);
            return Result.Fail<Guid>(initialBalanceResult.Error);
        }

        var result = Account.Create(command.Id, command.PartyId, initialBalanceResult.Value);

        if (result.IsFailure)
        {
            LogCreateAccountError(logger, result.Error, null);
            return Result.Fail<Guid>(result.Error);
        }

        var account = result.Value;
        await repository.SaveAsync(account, cancellationToken);

        LogAccountCreated(logger, account.Id, null);

        return Result.Ok(account.Id);
    }
}
