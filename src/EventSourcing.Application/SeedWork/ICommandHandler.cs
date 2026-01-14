namespace EventSourcing.Application.SeedWork;

using EventSourcing.Domain.Seedwork;
using System.Threading;
using System.Threading.Tasks;

public interface ICommandHandler<in TCommand, TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
