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
        if (a is null && b is null)
        {
            return new Money(0);
        }
        if (a is null)
        {
            return new Money(b!.Amount);
        }
        if (b is null)
        {
            return new Money(a.Amount);
        }
        return new(a.Amount + b.Amount);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a is null && b is null)
        {
            return new Money(0);
        }
        if (a is null)
        {
            return new Money(-b!.Amount);
        }
        if (b is null)
        {
            return new Money(a.Amount);
        }
        return new(a.Amount - b.Amount);
    }
}
