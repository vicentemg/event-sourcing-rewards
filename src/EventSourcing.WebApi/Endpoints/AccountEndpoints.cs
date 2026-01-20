namespace EventSourcing.WebApi.Endpoints;

using System.Threading.Tasks;
using EventSourcing.Application.SeedWork;
using AccountResponse = Application.Features.Account.Queries.GetAccount.Account;
using EventSourcing.Application.Features.Account.Queries.GetAccounts;
using EventSourcing.Application.Features.Account.Queries.GetAccount;
using EventSourcing.Domain.Aggregates.AccountAggregate;
using Microsoft.AspNetCore.Mvc;
using EventSourcing.Application.Features.Account.Commands.CreateAccount;
using EventSourcing.Application.Features.Account.Commands.IncurDebt;
using EventSourcing.Application.Features.Account.Commands.MakePayment;
using EventSourcing.Application.Features.Account.Commands.WithdrawFunds;
using EventSourcing.Application.Features.Account.Commands.DepositFounds;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        _ = app
        .MapGet("/accounts", GetAccountsAsync)
        .WithName("GetAccounts")
        .WithDescription("Get all accounts")
        .Produces<AccountResponse[]>(StatusCodes.Status200OK);

        _ = app
        .MapGet("/accounts/{id}", GetAccountByIdAsync)
        .WithName("GetAccount")
        .WithDescription("Get account by id")
        .Produces<AccountResponse>(StatusCodes.Status200OK);

        _ = app
        .MapPost("/accounts", CreateAccountAsync)
        .WithName("CreateAccount")
        .WithDescription("Create a new account")
        .Produces(StatusCodes.Status201Created);

        _ = app
        .MapPost("/accounts/{id}/deposit", DepositFundsAsync)
        .WithName("DepositFunds")
        .WithDescription("Deposit funds into an account")
        .Produces(StatusCodes.Status200OK);

        _ = app
        .MapPost("/accounts/{id}/withdraw", WithdrawFundsAsync)
        .WithName("WithdrawFunds")
        .WithDescription("Withdraw funds from an account")
        .Produces(StatusCodes.Status200OK);

        _ = app
        .MapPost("/accounts/{id}/incur-debt", IncurDebtAsync)
        .WithName("IncurDebt")
        .WithDescription("Incur debt on an account")
        .Produces(StatusCodes.Status200OK);

        _ = app
        .MapPost("/accounts/{id}/payment", MakePaymentAsync)
        .WithName("MakePayment")
        .WithDescription("Make a payment on an account")
        .Produces(StatusCodes.Status200OK);
    }

    internal static async Task<IResult> GetAccountsAsync([FromServices] IGetAccountsQueryHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetAccountsQuery(), cancellationToken);

        if (result.IsFailure)
        {
            return Results.NotFound(result.Error);
        }

        var response = result.Value.Select(account => account).ToList();

        return Results.Ok(response);
    }

    internal static async Task<IResult> GetAccountByIdAsync(Guid id, [FromServices] IGetAccountQueryHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetAccountQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return Results.NotFound(result.Error);
        }

        return Results.Ok(result.Value);
    }

    internal static async Task<IResult> CreateAccountAsync([FromBody] CreateAccountRequest request, [FromServices] ICommandHandler<CreateAccountCommand, Guid> handler, CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(Guid.NewGuid(), request.PartyId, request.InitialBalance);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }
        var accountId = result.Value;
        return Results.Created($"/accounts/{accountId}", new { Id = accountId });
    }

    internal static async Task<IResult> DepositFundsAsync(Guid id, [FromBody] DepositFundsRequest request, [FromServices] IDepositFundsCommandHandler handler)
    {
        var command = new DepositFundsCommand(id, request.Amount, request.MerchantName, request.MerchantType);
        var result = await handler.Handle(command, CancellationToken.None);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }

    internal static async Task<IResult> WithdrawFundsAsync(Guid id, [FromBody] WithdrawFundsRequest request, [FromServices] IWithdrawFundsCommandHandler handler)
    {
        var command = new WithdrawFundsCommand(id, request.Amount, request.MerchantName, request.MerchantType);
        var result = await handler.Handle(command, CancellationToken.None);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }

    internal static async Task<IResult> IncurDebtAsync(Guid id, [FromBody] IncurDebtRequest request, [FromServices] IIncurDebtCommandHandler handler)
    {
        var command = new IncurDebtCommand(id, request.Amount, request.MerchantName, request.MerchantType);
        var result = await handler.Handle(command, CancellationToken.None);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }

    internal static async Task<IResult> MakePaymentAsync(Guid id, [FromBody] MakePaymentRequest request, [FromServices] IMakePaymentCommandHandler handler)
    {
        var command = new MakePaymentCommand(id, request.Amount, request.MerchantName, request.MerchantType);
        var result = await handler.Handle(command, CancellationToken.None);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }
}

public record CreateAccountRequest(Guid PartyId, decimal InitialBalance);
public record DepositFundsRequest(decimal Amount, string MerchantName, VendorType MerchantType);
public record WithdrawFundsRequest(decimal Amount, string MerchantName, VendorType MerchantType);
public record IncurDebtRequest(decimal Amount, string MerchantName, VendorType MerchantType);
public record MakePaymentRequest(decimal Amount, string MerchantName, VendorType MerchantType);
