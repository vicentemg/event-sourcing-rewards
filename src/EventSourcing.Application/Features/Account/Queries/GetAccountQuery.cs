namespace EventSourcing.Application.Features.Account.Queries;

using EventSourcing.Domain.Aggregates.AccountAggregate;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record AccountDto(Guid Id, Guid PartyId, decimal Balance, AccountStatus Status);

public record GetAccountQuery(Guid AccountId);

public interface IGetAccountQueryHandler
{
    public Task<Result<AccountDto>> Handle(GetAccountQuery query, CancellationToken cancellationToken);
}

public class GetAccountQueryHandler(IRepository<Account> repository, ILogger<GetAccountQueryHandler> logger) : IGetAccountQueryHandler
{
    private static readonly Action<ILogger, Guid, Exception?> AccountNotFound =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(1, nameof(AccountNotFound)), "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, Guid, Exception?> HandlingGetAccountQuery =
        LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(2, nameof(HandlingGetAccountQuery)), "Handling GetAccountQuery for AccountId: {AccountId}");

    public async Task<Result<AccountDto>> Handle(GetAccountQuery query, CancellationToken cancellationToken)
    {
        HandlingGetAccountQuery(logger, query.AccountId, null);

        var account = await repository.GetAsync(query.AccountId, cancellationToken);
        if (account is null)
        {
            AccountNotFound(logger, query.AccountId, null);
            return Result.Fail<AccountDto>($"Account with ID {query.AccountId} not found.");
        }

        return Result.Ok(new AccountDto(account.Id, account.PartyId, account.Balance, account.Status));
    }
}
