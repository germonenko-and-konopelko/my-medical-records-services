namespace MMR.Patient.Common;

internal class PatientError(PatientErrorCode code)
{
    public readonly PatientErrorCode Code = code;

    public bool IsInternal => Code is PatientErrorCode.Internal or PatientErrorCode.Unknown;
}

internal enum PatientErrorCode
{
    Unknown = 0,
    PatientProfileExists = 101,
    PatientProfileDoesNotExist = 102,
    Internal = 999,
}