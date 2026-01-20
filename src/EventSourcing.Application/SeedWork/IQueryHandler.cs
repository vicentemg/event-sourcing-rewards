namespace EventSourcing.Application.SeedWork;

using EventSourcing.Domain.Seedwork;
using System.Threading;
using System.Threading.Tasks;

public interface IQueryHandler<in TQuery, TResult>
{
    Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
