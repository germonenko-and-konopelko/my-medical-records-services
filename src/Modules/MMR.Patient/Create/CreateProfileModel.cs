using FluentValidation;
using MMR.Common.Enums;

namespace MMR.Patient.Create;

public class CreateProfileModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Sex? Sex { get; set; }
}

public class CreateProfileModelValidator : AbstractValidator<CreateProfileModel>
{
    public CreateProfileModelValidator()
    {
        RuleFor(profile => profile.FirstName)
            .MaximumLength(50);

        RuleFor(profile => profile.LastName)
            .MaximumLength(50);

        RuleFor(profile => profile.Sex)
            .IsInEnum();
    }
}