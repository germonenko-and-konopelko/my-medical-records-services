using System.Diagnostics.CodeAnalysis;

namespace MMR.Common.Results;

public static class Result
{
    public static Result<TError> Ok<TError>()
        where TError : notnull
        => Result<TError>.FromResult();

    public static Result<TError> Err<TError>(TError error)
        where TError :notnull
        => Result<TError>.FromError(error);

    public static Result<TValue, TError> Ok<TValue, TError>(TValue result)
        where TValue : notnull
        where TError : notnull
        => Result<TValue, TError>.FromValue(result);

    public static Result<TValue, TError> Err<TValue, TError>(TError error)
        where TValue : notnull
        where TError : notnull
        => Result<TValue, TError>.FromError(error);

    public static async Task<Result<Exception>> TryAsync(Func<Task> func)
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

    public static Result<TValue, Exception> Try<TValue>(Func<TValue> func) where TValue : notnull
    {
        try
        {
            TValue result = func();
            return Ok<TValue, Exception>(result);
        }
        catch (Exception e)
        {
            return Err<TValue, Exception>(e);
        }
    }

    public static async Task<Result<TValue, Exception>> TryAsync<TValue>(Func<Task<TValue>> func) where TValue : notnull
    {
        try
        {
            TValue result = await func();
            return Ok<TValue, Exception>(result);
        }
        catch (Exception e)
        {
            return Err<TValue, Exception>(e);
        }
    }
}

public class Result<TError> where TError :notnull
{
    public readonly TError? Error;

    private Result(TError? error, bool isOk)
    {
        IsOk = isOk;
        Error = error;
    }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsOk { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => !IsOk;

    public static Result<TError> FromResult()
    {
        return new(default, isOk: true);
    }

    public static Result<TError> FromError(TError error)
    {
        return new(error, isOk: false);
    }
}

public sealed class Result<TValue, TError>
    where TValue : notnull
    where TError : notnull
{
    public readonly TValue? Value;
    public readonly TError? Error;

    private Result(TValue? value, TError? error, bool isOk)
    {
        if (value is null && error is null)
        {
            throw new ArgumentException("Both the result and the error cannot be null.");
        }

        IsOk = isOk;
        Value = value;
        Error = error;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsOk { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError => !IsOk;

    public Result<TValue, TError2> MapError<TError2>(Func<TError, TError2> mapFunc) where TError2 : notnull
    {
        if (IsOk)
        {
            return Result.Ok<TValue, TError2>(Value);
        }

        return Result.Err<TValue, TError2>(mapFunc(Error));
    }

    public Result<TValue, TError> Inspect(Action<TValue> inspectValueFunc, Action<TError> inspectErrorFunc)
    {
        if (IsOk)
        {
            inspectValueFunc(Value);
        }
        else
        {
            inspectErrorFunc(Error);
        }

        return this;
    }

    public static Result<TValue, TError> FromValue(TValue result)
    {
        return new(result, default, isOk: true);
    }

    public static Result<TValue, TError> FromError(TError error)
    {
        return new(default, error, isOk: false);
    }
}