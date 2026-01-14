namespace EventSourcing.Application.Features.Account.Queries.GetAccounts;

using EventSourcing.Application.Features.Account.Queries.GetAccount;
using EventSourcing.Domain.Seedwork;
using global::Marten;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public record GetAccountsQuery();

public interface IGetAccountsQueryHandler
{
    public Task<Result<IReadOnlyList<GetAccountModel>>> Handle(GetAccountsQuery query, CancellationToken cancellationToken);
}

public class GetAccountsQueryHandler(IQuerySession querySession, ILogger<GetAccountsQueryHandler> logger) : IGetAccountsQueryHandler
{
    private static readonly Action<ILogger, Exception?> HandlingGetAccountsQuery =
        LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(HandlingGetAccountsQuery)), "Handling GetAccountsQuery");

    public async Task<Result<IReadOnlyList<GetAccountModel>>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        HandlingGetAccountsQuery(logger, null);

        var accounts = await querySession.Query<GetAccountModel>().ToListAsync(token: cancellationToken);

        return Result.Ok((IReadOnlyList<GetAccountModel>)accounts);
    }
}
