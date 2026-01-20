namespace EventSourcing.Domain.Seedwork;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Fail(string message)
    {
        return new(false, message);
    }

    public static Result<T> Fail<T>(string message)
    {
        return new(default!, false, message);
    }

    public static Result Ok()
    {
        return new(true, string.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new(value, true, string.Empty);
    }

}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static explicit operator T(Result<T> result)
    {
        return result.Value;
    }
}
