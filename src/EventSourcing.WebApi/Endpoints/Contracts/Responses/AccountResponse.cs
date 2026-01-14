namespace EventSourcing.WebApi.Endpoints.Contracts.Responses;
using EventSourcing.Application.Features.Account.Queries.GetAccount;

/// <summary>
/// Response DTO for Account. Maps from AccountReadModel to the public API contract.
/// </summary>
public record AccountResponse(Guid Id, Guid PartyId, decimal Balance, string Status)
{
    public static explicit operator AccountResponse(GetAccountModel readModel)
    {
        return new AccountResponse(
            readModel.Id,
            readModel.PartyId,
            readModel.Balance,
            readModel.Status.ToString()
        );
    }
};
