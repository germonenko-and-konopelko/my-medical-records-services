using MMR.Common.Results;
using MMR.Patient.Create;

namespace MMR.Patient.Common;

internal interface IPatientProfileRepository
{
    internal Task<Result<bool, Exception>> UserHasProfileAsync(string userId);

    internal Task<Result<Exception>> SaveAsync(Profile profile);
}