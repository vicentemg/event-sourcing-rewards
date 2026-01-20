namespace EventSourcing.Application.Features.Account.Queries.GetAccounts;

using EventSourcing.Application.Features.Account.Queries.GetAccount;
using EventSourcing.Domain.Seedwork;
using EventSourcing.Application.SeedWork;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public record GetAccountsQuery();

public interface IGetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, IReadOnlyList<Account>>
{
}

public class GetAccountsQueryHandler(IProjectionRepository<Account> repository, ILogger<GetAccountsQueryHandler> logger) : IGetAccountsQueryHandler
{
    private static readonly Action<ILogger, Exception?> HandlingGetAccountsQuery =
        LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(HandlingGetAccountsQuery)), "Handling GetAccountsQuery");

    public async Task<Result<IReadOnlyList<Account>>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        HandlingGetAccountsQuery(logger, null);

        var accounts = await repository.GetAllAsync(cancellationToken);

        return Result.Ok(accounts);
    }
}
