namespace EventSourcing.Application.Features.Account.Commands;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record DepositFundsCommand(Guid AccountId, decimal Amount, string MerchantName, VendorType MerchantType);

public interface IDepositFundsCommandHandler
{
    public Task<Result> Handle(DepositFundsCommand command, CancellationToken cancellationToken = default);
}

public class DepositFundsCommandHandler(IRepository<Account> repository, ILogger<DepositFundsCommandHandler> logger) : IDepositFundsCommandHandler
{
    private static readonly Action<ILogger, Guid, decimal, Exception?> LogHandlingDepositFundsCommand =
        LoggerMessage.Define<Guid, decimal>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling DepositFundsCommand for AccountId: {AccountId}, Amount: {Amount}");

    private static readonly Action<ILogger, Guid, Exception?> LogAccountNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(2, "AccountNotFound"),
            "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, string, Exception?> LogDepositFundsError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, "DepositFundsError"),
            "Failed to deposit funds: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogFundsDeposited =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(4, "FundsDeposited"),
            "Funds deposited to account with ID: {AccountId}");

    public async Task<Result> Handle(DepositFundsCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingDepositFundsCommand(logger, command.AccountId, command.Amount, null);

        var account = await repository.GetAsync(command.AccountId, cancellationToken);
        if (account is null)
        {
            LogAccountNotFound(logger, command.AccountId, null);
            return Result.Fail("Account with ID " + command.AccountId + " not found.");
        }

        var amountResult = Money.Create(command.Amount);
        if (amountResult.IsFailure)
        {
            LogDepositFundsError(logger, amountResult.Error, null);
            return Result.Fail(amountResult.Error);
        }

        var merchant = new Merchant(command.MerchantName, command.MerchantType);
        var result = account.Deposit(amountResult.Value, merchant);

        if (result.IsFailure)
        {
            LogDepositFundsError(logger, result.Error, null);
            return result;
        }

        await repository.SaveAsync(account, cancellationToken);

        LogFundsDeposited(logger, account.Id, null);

        return Result.Ok();
    }
}