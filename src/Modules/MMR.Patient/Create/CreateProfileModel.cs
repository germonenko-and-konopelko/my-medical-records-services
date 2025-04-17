using FluentValidation;
using Microsoft.Extensions.Localization;
using MMR.Common;
using MMR.Common.Api.Validation;
using MMR.Common.Enums;
using MMR.Patient.Resources;

namespace MMR.Patient.Create;

internal class CreateProfileModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Sex? Sex { get; set; }
}

internal class CreateProfileModelValidator : AbstractValidator<CreateProfileModel>
{
    private static readonly DateOnly BirthdateThreshold = new(1930, 01, 01);

    private static readonly NoControlCharactersValidator<CreateProfileModel> NoControlCharactersValidator = new();

    public CreateProfileModelValidator(IStringLocalizer<ErrorMessages> localizer, TimeProvider timeProvider)
    {
        RuleFor(profile => profile.FirstName)
            .MaximumLength(50)
            .SetValidator(NoControlCharactersValidator);

        RuleFor(profile => profile.LastName)
            .MaximumLength(50)
            .SetValidator(NoControlCharactersValidator);

        RuleFor(profile => profile.BirthDate)
            .Must(birthDate => !birthDate.HasValue || birthDate.Value < timeProvider.GetUtcDateNow())
            .WithMessage(_ => localizer["MustBePastDate"])
            .Must(birthDate => !birthDate.HasValue || birthDate.Value >= BirthdateThreshold)
            .WithMessage(_ => localizer["MinBirthdayMessage"]);

        RuleFor(profile => profile.Sex)
            .Must(sex => sex is Sex.Male or Sex.Female)
            .When(profile => profile.Sex is not null)
            .WithMessage(_ => localizer["MustBeMaleOrFemale"]);
    }
}