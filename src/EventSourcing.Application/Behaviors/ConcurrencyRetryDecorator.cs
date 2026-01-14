namespace EventSourcing.Application.Behaviors;

using EventSourcing.Application.SeedWork;
using EventSourcing.Domain.Seedwork;
using global::Polly;
using System.Threading;
using System.Threading.Tasks;

public class ConcurrencyRetryDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> inner,
    AsyncPolicy policy)
    : ICommandHandler<TCommand, TResult>
{
    public async Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        return await policy.ExecuteAsync(async (token) =>
        {
            return await inner.Handle(command, token);
        }, cancellationToken);
    }
}
