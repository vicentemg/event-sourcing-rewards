namespace EventSourcing.Infrastructure.Behaviors;

using EventSourcing.Application.SeedWork;
using EventSourcing.Domain.Seedwork;
using Polly;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public partial class ConcurrencyRetryDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> inner,
    AsyncPolicy policy,
    ILogger<ConcurrencyRetryDecorator<TCommand, TResult>> logger)
    : ICommandHandler<TCommand, TResult>
{
    public async Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        return await policy.ExecuteAsync(async (token) =>
        {
            try
            {
                return await inner.Handle(command, token);
            }
            catch (Exception ex)
            {
                LogConcurrencyError(logger, ex);
                throw;
            }
        }, cancellationToken);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Error in ConcurrencyRetryDecorator")]
    private static partial void LogConcurrencyError(ILogger logger, Exception ex);
}
