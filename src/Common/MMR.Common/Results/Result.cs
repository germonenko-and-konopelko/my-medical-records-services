using System.Diagnostics;

namespace MMR.Common.Results;

public static class Result
{
    public static Result<TError> Ok<TError>() => Result<TError>.FromResult();

    public static Result<TError> Err<TError>(TError error) => Result<TError>.FromError(error);

    public static Result<TResult, TError> Ok<TResult, TError>(TResult result)
        => Result<TResult, TError>.FromResult(result);

    public static Result<TResult, TError> Err<TResult, TError>(TError error)
        => Result<TResult, TError>.FromError(error);

    public static async Task<Result<Exception>> SafelyAsync(Func<Task> func)
    {
        try
        {
            await func();
            return Ok<Exception>();
        }
        catch (Exception e)
        {
            return Err(e);
        }
    }
}

public class Result<TError>
{
    private readonly TError? _error;

    private Result(TError? error)
    {
        _error = error;
    }

    public bool IsError => _error is null;

    public static Result<TError> FromResult()
    {
        return new(default);
    }

    public static Result<TError> FromError(TError error)
    {
        return new(error);
    }
}

public class Result<TResult, TError>
{
    private readonly TResult? _result;
    private readonly TError? _error;

    private Result(TResult? result, TError? error)
    {
        if (result is null && error is null)
        {
            throw new ArgumentException("Both the result and the error cannot be null.");
        }

        if (result is not null && error is not null)
        {
            throw new ArgumentException("The result or the error must be null.");
        }

        _result = result;
        _error = error;
    }

    public bool IsOk => _result is null;

    public bool IsError => _error is null;

    public TResult Unwrap() => _result ?? throw new UnwrapException($"Failed to unwrap a result of type {GetType()}.");

    public TResult UnwrapOr(TResult defaultValue)
    {
        return _result ?? defaultValue;
    }

    public static Result<TResult, TError> FromResult(TResult result)
    {
        return new(result, default);
    }

    public static Result<TResult, TError> FromError(TError error)
    {
        return new(default, error);
    }
}