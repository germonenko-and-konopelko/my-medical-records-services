using FluentValidation;
using FluentValidation.Validators;

namespace MMR.Common.Api.Validation;

public class NoControlCharactersValidator<T> : PropertyValidator<T, string?>
{
    public override string Name => "NoControlCharacters";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (value is null || value.Length == 0)
        {
            return true;
        }

        return value.All(c => !char.IsControl(c));
    }
}