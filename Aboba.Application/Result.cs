
namespace Aboba.Application;

public record Result
{
    protected Error? Error { get; set; }
    public bool IsSuccess() => Error == null;
    public static Result FromSuccess() => new();

    public static Result FromError(string message)
    {
        return new Result()
        {
            Error = new Error(message)
        };
    }
    internal Result(){}
    
    private Result(Error error)
    {
        Error = error;
    }

    public static implicit operator Result(Error error) => new(error);
}

public record Result<T> : Result
{
    public T? Value { get; private init; }

    public static Result<T> FromSuccess(T value)
    {
        return new Result<T>(value);
    }
    
    public new static Result<T> FromError(string message)
    {
        return new Result<T>(new Error(message  ));
    }

    private Result(Error error)
    {
        Error = error;
    }
    
    private Result(T value)
    {
        Value = value;
    }
    
    public static implicit operator Result<T>(T value) => FromSuccess(value);
    public static implicit operator Result<T>(Error error) => new(error);
}

public record Error(string Message);