namespace EventSourcing.Domain.Seedwork;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        this.IsSuccess = isSuccess;
        this.Error = error;
    }

    public static Result Fail(string message) => new(false, message);

    public static Result<T> Fail<T>(string message) => new(default!, false, message);

    public static Result Ok() => new(true, string.Empty);

    public static Result<T> Ok<T>(T value) => new(value, true, string.Empty);

}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error) => this.Value = value;

    public static explicit operator T(Result<T> result) => result.Value;
}
