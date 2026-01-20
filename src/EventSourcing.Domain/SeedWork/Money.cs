namespace EventSourcing.Domain.Seedwork;

public record Money
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        Amount = amount;
    }

    public static Result<Money> Create(decimal amount)
    {
        if (amount < 0)
        {
            return Result.Fail<Money>("Amount cannot be negative.");
        }

        return Result.Ok(new Money(amount));
    }

    public static implicit operator decimal(Money money)
    {
        return money.Amount;
    }

    public static Money operator +(Money a, Money b)
    {
        return new(a.Amount + b.Amount);
    }

    public static Money operator -(Money a, Money b)
    {
        return new(a.Amount - b.Amount);
    }
}
