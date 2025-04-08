using MMR.Common.Api;
using MMR.Common.Results;
using MMR.Patient.Common;

namespace MMR.Patient.Create;

internal interface IPatientProfileCreator
{
    internal Task<Result<Profile, PatientError>> CreateAsync(CreateProfileModel createProfileModel);
}

internal class PatientProfileCreator(IPatientProfileRepository repository, UserContext userContext)
    : IPatientProfileCreator
{
    private PatientError InternalError => new(PatientErrorCode.Internal);

    public async Task<Result<Profile, PatientError>> CreateAsync(CreateProfileModel createProfileModel)
    {
        var userId = userContext.CurrentUserId ?? string.Empty;
        var userHasProfile = await repository.UserHasProfileAsync(userId);
        if (userHasProfile.IsError)
        {
            return Result.Err<Profile, PatientError>(InternalError);
        }

        var profileIsInUse = userHasProfile.UnwrapOr(true);
        if (profileIsInUse)
        {
            var error = new PatientError(PatientErrorCode.PatientProfileExists);
            return Result.Err<Profile, PatientError>(error);
        }

        var profile = new Profile
        {
            UserId = userId,
            FirstName = createProfileModel.FirstName,
            LastName = createProfileModel.LastName,
            BirthDate = createProfileModel.BirthDate,
            Sex = createProfileModel.Sex,
        };
        var saveResult = await repository.SaveAsync(profile);
        if (saveResult.IsError)
        {
            return Result.Err<Profile, PatientError>(InternalError);
        }

        return Result.Ok<Profile, PatientError>(profile);
    }
}