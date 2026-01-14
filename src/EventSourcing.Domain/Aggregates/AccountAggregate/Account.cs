namespace EventSourcing.Domain.Aggregates.AccountAggregate;

using EventSourcing.Domain.Aggregates.AccountAggregate.Events;
using EventSourcing.Domain.Seedwork;

public class Account : AggregateRoot
{
    public Account() { }
    public Account(Guid id)
    {
        Id = id;
    }

    public Guid PartyId { get; private set; }
    public Money Balance { get; private set; }
    public Money Debt { get; private set; }
    public AccountStatus Status { get; private set; }


    public static Result<Account> Create(Guid accountId, Guid partyId, Money initialBalance)
    {
        var account = new Account(accountId)
        {
            PartyId = partyId,
            Balance = Money.Create(0).Value,
            Debt = Money.Create(0).Value
        };

        account.RaiseEvent(new AccountCreated(accountId, partyId, DateTime.UtcNow));

        if (initialBalance.Amount > 0)
        {
            account.RaiseEvent(new FundsDeposited(account.Id, initialBalance, new Merchant("System", VendorType.Other), DateTime.UtcNow));
        }

        return Result.Ok(account);
    }

    public Result Deposit(Money amount, Merchant merchant)
    {
        if (Status == AccountStatus.Closed)
        {
            return Result.Fail("Cannot deposit funds into a closed account.");
        }

        RaiseEvent(new FundsDeposited(Id, amount, merchant, DateTime.UtcNow));
        return Result.Ok();
    }

    public Result Withdraw(Money amount, Merchant merchant)
    {
        if (Balance.Amount < amount.Amount)
        {
            return Result.Fail("Insufficient funds.");
        }
        if (Status != AccountStatus.Active)
        {
            return Result.Fail("Account is not active.");
        }

        RaiseEvent(new FundsWithdrawn(Id, amount, merchant, DateTime.UtcNow));
        return Result.Ok();
    }

    public Result IncurDebt(Money amount, Merchant merchant)
    {
        if (Status != AccountStatus.Active)
        {
            return Result.Fail("Account is not active.");
        }

        RaiseEvent(new DebtIncurred(Id, amount, merchant, DateTime.UtcNow));
        return Result.Ok();
    }

    internal void Apply(AccountCreated e)
    {
        Id = e.AccountId;
        PartyId = e.PartyId;
        Balance = Money.Create(0).Value;
        Debt = Money.Create(0).Value;
        Status = AccountStatus.Active;
    }

    internal void Apply(FundsDeposited e)
    {
        Balance += e.Amount;
    }

    internal void Apply(FundsWithdrawn e)
    {
        Balance -= e.Amount;
    }

    internal void Apply(DebtIncurred e)
    {
        Debt += e.Amount;
    }
}

public enum AccountStatus
{
    Active,
    Closed,
    Frozen
}
