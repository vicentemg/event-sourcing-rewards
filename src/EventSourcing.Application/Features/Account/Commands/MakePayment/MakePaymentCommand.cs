namespace EventSourcing.Application.Features.Account.Commands.MakePayment;

using EventSourcing.Application.SeedWork;
using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record MakePaymentCommand(Guid AccountId, decimal Amount, string MerchantName, VendorType MerchantType);

public interface IMakePaymentCommandHandler : ICommandHandler<MakePaymentCommand, Unit>
{
}

public class MakePaymentCommandHandler(IAggregateRepository<Account> repository, ILogger<MakePaymentCommandHandler> logger) : IMakePaymentCommandHandler
{
    private static readonly Action<ILogger, Guid, decimal, Exception?> LogHandlingMakePaymentCommand =
        LoggerMessage.Define<Guid, decimal>(
            LogLevel.Information,
            new EventId(1, nameof(Handle)),
            "Handling MakePaymentCommand for AccountId: {AccountId}, Amount: {Amount}");

    private static readonly Action<ILogger, Guid, Exception?> LogAccountNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(2, "AccountNotFound"),
            "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, string, Exception?> LogMakePaymentError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, "MakePaymentError"),
            "Failed to make payment: {Error}");

    private static readonly Action<ILogger, Guid, Exception?> LogPaymentMade =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(4, "PaymentMade"),
            "Payment made for account with ID: {AccountId}");

    public async Task<Result<Unit>> Handle(MakePaymentCommand command, CancellationToken cancellationToken = default)
    {
        LogHandlingMakePaymentCommand(logger, command.AccountId, command.Amount, null);

        var accountResult = await GetAccountAsync(command.AccountId, cancellationToken);

        if (accountResult.IsFailure)
        {
            return Result.Fail<Unit>(accountResult.Error);
        }

        var account = accountResult.Value;

        var amountResult = Money.Create(command.Amount);
        if (amountResult.IsFailure)
        {
            LogMakePaymentError(logger, amountResult.Error, null);
            return Result.Fail<Unit>(amountResult.Error);
        }
        var paymentAmount = amountResult.Value;

        var merchant = new Merchant(command.MerchantName, command.MerchantType);

        var paymentResult = MakePayment(account, paymentAmount, merchant);
        if (paymentResult.IsFailure)
        {
            LogMakePaymentError(logger, paymentResult.Error, null);
            return Result.Fail<Unit>(paymentResult.Error);
        }

        await repository.SaveAsync(account, cancellationToken);

        LogPaymentMade(logger, account.Id, null);

        return Result.Ok(Unit.Value);
    }

    private async Task<Result<Account>> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await repository.LoadAsync(accountId, cancellationToken);
        if (account is null)
        {
            LogAccountNotFound(logger, accountId, null);
            return Result.Fail<Account>("Account with ID " + accountId + " not found.");
        }
        return Result.Ok(account);
    }

    private static Result MakePayment(Account account, Money paymentAmount, Merchant merchant)
    {
        if (account.Balance.Amount >= paymentAmount.Amount)
        {
            return account.Withdraw(paymentAmount, merchant);
        }

        var amountToWithdraw = account.Balance;
        var amountToIncurDebt = paymentAmount - amountToWithdraw;

        if (amountToWithdraw.Amount > 0)
        {
            var withdrawResult = account.Withdraw(amountToWithdraw, merchant);
            if (withdrawResult.IsFailure)
            {
                return withdrawResult;
            }
        }

        return account.IncurDebt(amountToIncurDebt, merchant);
    }
}
