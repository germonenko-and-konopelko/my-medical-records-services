namespace MMR.Patient.Common;

internal class PatientError(PatientErrorCode code)
{
    public readonly PatientErrorCode Code = code;
}

internal enum PatientErrorCode
{
    Unknown = 0,
    PatientProfileExists = 101,
    Internal = 999,
}