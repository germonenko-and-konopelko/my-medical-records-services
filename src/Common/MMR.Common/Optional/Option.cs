using System.Diagnostics.CodeAnalysis;

namespace MMR.Common.Optional;

public readonly struct Option<T>
    where T : notnull
{
    private readonly bool _hasValue = false;

    public readonly T? Value = default;

    public Option()
    {
        _hasValue = false;
        Value = default;
    }

    private Option(T value)
    {
        _hasValue = true;
        Value = value;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSome => _hasValue;

    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsNone => !_hasValue;

    public static Option<T> Some(T value)
    {
        return new(value);
    }

    public static Option<T> None() => new();
}