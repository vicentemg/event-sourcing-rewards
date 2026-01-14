namespace EventSourcing.Application.Features.Account.Commands.IncurDebt;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record IncurDebtCommand(Guid AccountId, decimal Amount, string MerchantName, VendorType MerchantType);

public interface IIncurDebtCommandHandler
{
    public Task<Result> Handle(IncurDebtCommand command, CancellationToken cancellationToken = default);
}

public class IncurDebtCommandHandler(IAggregateRepository<Account> repository, ILogger<IncurDebtCommandHandler> logger) : IIncurDebtCommandHandler
{
    private static readonly Action<ILogger, Guid, decimal, Exception?> LogHandlingIncurDebtCommand =
        LoggerMessage.Define<Guid, decimal>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling IncurDebtCommand for AccountId: {AccountId}, Amount: {Amount}");

    private static readonly Action<ILogger, Guid, Exception?> LogAccountNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(2, "AccountNotFound"),
            "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, string, Exception?> LogIncurDebtError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, "IncurDebtError"),
            "Failed to incur debt: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogDebtIncurred =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(4, "DebtIncurred"),
            "Debt incurred for account with ID: {AccountId}");

    public async Task<Result> Handle(IncurDebtCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingIncurDebtCommand(logger, command.AccountId, command.Amount, null);

        var account = await repository.LoadAsync(command.AccountId, cancellationToken);
        if (account is null)
        {
            LogAccountNotFound(logger, command.AccountId, null);
            return Result.Fail("Account with ID " + command.AccountId + " not found.");
        }

        var amountResult = Money.Create(command.Amount);
        if (amountResult.IsFailure)
        {
            LogIncurDebtError(logger, amountResult.Error, null);
            return Result.Fail(amountResult.Error);
        }

        var merchant = new Merchant(command.MerchantName, command.MerchantType);
        var result = account.IncurDebt(amountResult.Value, merchant);

        if (result.IsFailure)
        {
            LogIncurDebtError(logger, result.Error, null);
            return result;
        }

        await repository.SaveAsync(account, cancellationToken);

        LogDebtIncurred(logger, account.Id, null);

        return Result.Ok();
    }
}
