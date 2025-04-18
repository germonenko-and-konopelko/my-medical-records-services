namespace MMR.Common.Encoding;

public class EncoderError(
    EncoderErrorCode errorCode,
    Exception? innerException = null
)
{
    public readonly EncoderErrorCode Code = errorCode;

    public readonly Exception? InnerException = innerException;
}

public enum EncoderErrorCode
{
    Unknown = 0,
    InputNumberLessThanZero = 1,
    InputStringIsEmpty = 2,
    IncorrectInputString = 3,
    InnerError = 999,
}