namespace EventSourcing.Application.Features.Account.Queries.GetAccount;

using EventSourcing.Application.SeedWork;
using EventSourcing.Domain.Seedwork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public record GetAccountQuery(Guid AccountId);

public interface IGetAccountQueryHandler : IQueryHandler<GetAccountQuery, Account>
{
}

public class GetAccountQueryHandler(IProjectionRepository<Account> repository, ILogger<GetAccountQueryHandler> logger) : IGetAccountQueryHandler
{
    private static readonly Action<ILogger, Guid, Exception?> AccountNotFound =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(1, nameof(AccountNotFound)), "Account with ID {AccountId} not found.");

    private static readonly Action<ILogger, Guid, Exception?> HandlingGetAccountQuery =
        LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(2, nameof(HandlingGetAccountQuery)), "Handling GetAccountQuery for AccountId: {AccountId}");

    public async Task<Result<Account>> Handle(GetAccountQuery query, CancellationToken cancellationToken)
    {
        HandlingGetAccountQuery(logger, query.AccountId, null);

        var account = await repository.GetByIdAsync(query.AccountId, cancellationToken);
        if (account is null)
        {
            AccountNotFound(logger, query.AccountId, null);
            return Result.Fail<Account>($"Account with ID {query.AccountId} not found.");
        }

        return Result.Ok(account);
    }
}
