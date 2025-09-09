namespace EventSourcing.Domain.Aggregates.AccountAggregate;

using EventSourcing.Domain.Aggregates.AccountAggregate.Events;
using EventSourcing.Domain.Seedwork;

public class Account : AggregateRoot
{
    public Account() { }
    public Account(Guid id) => this.Id = id;

    public Guid PartyId { get; private set; }
    public decimal Balance { get; private set; }
    public AccountStatus Status { get; private set; }


    public static Result<Account> Create(Guid accountId, Guid partyId, decimal initialBalance = 0)
    {
        if (initialBalance < 0)
        {
            return Result.Fail<Account>("Initial balance cannot be negative.");
        }

        var account = new Account(accountId)
        {
            PartyId = partyId
        };

        account.RaiseEvent(new AccountCreated(Guid.NewGuid(), accountId, DateTime.UtcNow));

        if (initialBalance > 0)
        {
            account.RaiseEvent(new FundsDeposited(account.Id, initialBalance, DateTime.UtcNow));
        }

        return Result.Ok(account);
    }

    public Result Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Fail("Deposit amount must be positive.");
        }
        if (this.Status == AccountStatus.Closed)
        {
            return Result.Fail("Cannot deposit funds into a closed account.");
        }

        this.RaiseEvent(new FundsDeposited(this.Id, amount, DateTime.UtcNow));
        return Result.Ok();
    }

    public Result Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Fail("Withdrawal amount must be positive.");
        }
        if (this.Balance < amount)
        {
            return Result.Fail("Insufficient funds.");
        }
        if (this.Status != AccountStatus.Active)
        {
            return Result.Fail("Account is not active.");
        }

        this.RaiseEvent(new FundsWithdrawn(this.Id, amount, DateTime.UtcNow));
        return Result.Ok();
    }

    // The Apply methods are private and are called via dynamic dispatch from the AggregateRoot.
    // This is a convention for applying events to the aggregate's state.
    private void Apply(AccountCreated e)
    {
        this.Id = e.AccountId;
        this.PartyId = e.PartyId;
        this.Balance = 0;
        this.Status = AccountStatus.Active;
    }

    private void Apply(FundsDeposited e) => this.Balance += e.Amount;

    private void Apply(FundsWithdrawn e) => this.Balance -= e.Amount;

    private void Apply(object _)
    {
        // Fallback for any unhandled events.
    }
}

public enum AccountStatus
{
    Active,
    Closed,
    Frozen
}
