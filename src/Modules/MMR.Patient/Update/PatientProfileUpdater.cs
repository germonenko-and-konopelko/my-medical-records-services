using MMR.Common.Optional;
using MMR.Common.Results;
using MMR.Patient.Common;

namespace MMR.Patient.Update;

internal interface IPatientProfileUpdater
{
    internal Task<Result<Profile, PatientError>> CreateAsync(string userId, UpdateProfileModel updateProfileModel);
}

internal class PatientProfileUpdater(IPatientProfileRepository repository) : IPatientProfileUpdater
{
    private PatientError InternalError => new(PatientErrorCode.Internal);

    public async Task<Result<Profile, PatientError>> CreateAsync(
        string userId,
        UpdateProfileModel updateProfileModel
    )
    {
        Result<Option<Profile>, Exception> patientProfileResult = await repository.GetProfileAsync(userId);
        if (patientProfileResult.IsError)
        {
            return Result.Err<Profile, PatientError>(InternalError);
        }

        if (patientProfileResult.Value.IsNone)
        {
            PatientError error = new(PatientErrorCode.PatientProfileDoesNotExist);
            return Result.Err<Profile, PatientError>(error);
        }

        Profile patientProfile = patientProfileResult.Value.Value;
        updateProfileModel.ApplyChanges(patientProfile);

        Result<Exception> saveResult = await repository.SaveAsync(patientProfile);
        return saveResult.IsError
            ? Result.Err<Profile, PatientError>(InternalError)
            : Result.Ok<Profile, PatientError>(patientProfile);
    }
}