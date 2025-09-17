namespace EventSourcing.Application.Features.Account.Commands;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record WithdrawFundsCommand(Guid AccountId, decimal Amount, string MerchantName, VendorType MerchantType);

public interface IWithdrawFundsCommandHandler
{
    public Task<Result> Handle(WithdrawFundsCommand command, CancellationToken cancellationToken = default);
}

public class WithdrawFundsCommandHandler(IRepository<Account> repository, ILogger<WithdrawFundsCommandHandler> logger) : IWithdrawFundsCommandHandler
{
    private static readonly Action<ILogger, Guid, decimal, Exception?> LogHandlingWithdrawFundsCommand =
        LoggerMessage.Define<Guid, decimal>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling WithdrawFundsCommand for AccountId: {AccountId}, Amount: {Amount}");

    private static readonly Action<ILogger, Guid, Exception?> LogAccountNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(2, "AccountNotFound"),
            "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, string, Exception?> LogWithdrawFundsError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, "WithdrawFundsError"),
            "Failed to withdraw funds: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogFundsWithdrawn =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(4, "FundsWithdrawn"),
            "Funds withdrawn from account with ID: {AccountId}");

    public async Task<Result> Handle(WithdrawFundsCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingWithdrawFundsCommand(logger, command.AccountId, command.Amount, null);

        var account = await repository.GetAsync(command.AccountId, cancellationToken);
        if (account is null)
        {
            LogAccountNotFound(logger, command.AccountId, null);
            return Result.Fail("Account with ID " + command.AccountId + " not found.");
        }

        var amountResult = Money.Create(command.Amount);
        if (amountResult.IsFailure)
        {
            LogWithdrawFundsError(logger, amountResult.Error, null);
            return Result.Fail(amountResult.Error);
        }

        var merchant = new Merchant(command.MerchantName, command.MerchantType);
        var result = account.Withdraw(amountResult.Value, merchant);

        if (result.IsFailure)
        {
            LogWithdrawFundsError(logger, result.Error, null);
            return result;
        }

        await repository.SaveAsync(account, cancellationToken);

        LogFundsWithdrawn(logger, account.Id, null);

        return Result.Ok();
    }
}