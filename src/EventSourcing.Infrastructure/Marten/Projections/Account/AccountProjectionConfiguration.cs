namespace EventSourcing.Infrastructure.Marten.Projections.Account;

using EventSourcing.Infrastructure.Marten.Configuration;
using global::Marten;
using JasperFx.Events.Projections;
using EventSourcing.Application.Features.Account.Queries.GetAccount;

public class AccountProjectionConfiguration : IProjectionConfiguration
{
    public void Configure(StoreOptions options)
    {
        _ = options.Schema
            .For<Account>()
            .DocumentAlias("accountreadmodel");

        options.Projections.Add<AccountProjection>(ProjectionLifecycle.Async);
    }
}
