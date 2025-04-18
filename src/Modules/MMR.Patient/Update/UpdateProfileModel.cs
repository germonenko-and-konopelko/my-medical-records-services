using FluentValidation;
using Microsoft.Extensions.Localization;
using MMR.Common;
using MMR.Common.Api;
using MMR.Common.Api.Validation;
using MMR.Common.Enums;
using MMR.Patient.Common;
using MMR.Patient.Create;
using MMR.Patient.Resources;

namespace MMR.Patient.Update;

public class UpdateProfileModel
{
    public PossiblyUndefined<string?> FirstName { get; set; }

    public PossiblyUndefined<string?> LastName { get; set; }

    public PossiblyUndefined<DateOnly?> BirthDate { get; set; }

    public PossiblyUndefined<Sex?> Sex { get; set; }

    public void ApplyChanges(Profile profile)
    {
        if (FirstName.Defined)
        {
            profile.FirstName = FirstName.Value;
        }

        if (LastName.Defined)
        {
            profile.LastName = LastName.Value;
        }

        if (BirthDate.Defined)
        {
            profile.BirthDate = BirthDate.Value;
        }

        if (Sex.Defined)
        {
            profile.Sex = Sex.Value;
        }
    }
}

internal class UpdateProfileModelValidator : AbstractValidator<UpdateProfileModel>
{
    private static readonly DateOnly BirthdateThreshold = new(1930, 01, 01);

    private static readonly NoControlCharactersValidator<UpdateProfileModel> NoControlCharactersValidator = new();

    public UpdateProfileModelValidator(IStringLocalizer<ErrorMessages> localizer, TimeProvider timeProvider)
    {
        RuleFor(profile => profile.FirstName.Value)
            .MaximumLength(50)
            .SetValidator(NoControlCharactersValidator)
            .When(profile => profile.FirstName.Defined)
            .OverridePropertyName(nameof(CreateProfileModel.FirstName));

        RuleFor(profile => profile.LastName.Value)
            .MaximumLength(50)
            .SetValidator(NoControlCharactersValidator)
            .When(profile => profile.FirstName.Defined)
            .OverridePropertyName(nameof(CreateProfileModel.LastName));

        RuleFor(profile => profile.BirthDate.Value)
            .Must(birthDate => !birthDate.HasValue || birthDate.Value < timeProvider.GetUtcDateNow())
            .WithMessage(_ => localizer["MustBePastDate"])
            .Must(birthDate => !birthDate.HasValue || birthDate.Value >= BirthdateThreshold)
            .WithMessage(_ => localizer["MinBirthdayMessage"])
            .OverridePropertyName(nameof(CreateProfileModel.BirthDate));

        RuleFor(profile => profile.Sex)
            .Must(sex => sex.Value is Sex.Male or Sex.Female)
            .When(profile => profile.Sex is { Defined: true, Value: not null })
            .WithMessage(_ => localizer["MustBeMaleOrFemale"]);
    }
}