using MMR.Common.Results;
using MMR.Patient.Common;

namespace MMR.Patient.Create;

internal interface IPatientProfileCreator
{
    internal Task<Result<Profile, PatientError>> CreateAsync(string userId, CreateProfileModel createProfileModel);
}

internal class PatientProfileCreator(IPatientProfileRepository repository) : IPatientProfileCreator
{
    private PatientError InternalError => new(PatientErrorCode.Internal);

    public async Task<Result<Profile, PatientError>> CreateAsync(string userId, CreateProfileModel createProfileModel)
    {
        Result<bool, Exception>? userHasProfile = await repository.PatientHasProfileAsync(userId);
        if (userHasProfile.IsError)
        {
            return Result.Err<Profile, PatientError>(InternalError);
        }

        if (userHasProfile.Value)
        {
            PatientError? error = new PatientError(PatientErrorCode.PatientProfileExists);
            return Result.Err<Profile, PatientError>(error);
        }

        Profile? profile = new Profile
        {
            UserId = userId,
            FirstName = createProfileModel.FirstName,
            LastName = createProfileModel.LastName,
            BirthDate = createProfileModel.BirthDate,
            Sex = createProfileModel.Sex,
        };
        Result<Exception>? saveResult = await repository.SaveAsync(profile);
        if (saveResult.IsError)
        {
            return Result.Err<Profile, PatientError>(InternalError);
        }

        return Result.Ok<Profile, PatientError>(profile);
    }
}