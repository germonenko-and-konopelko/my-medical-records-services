using MMR.Common.Results;
using Sqids;

namespace MMR.Common.Encoding;

public interface IEncoder
{
    public Result<string, EncoderError> Encode(long source);

    public Result<long, EncoderError> Decode(string source);
}

public class Encoder(SqidsEncoder<long> innerEncoder) : IEncoder
{
    public Result<string, EncoderError> Encode(long source)
    {
        if (source < 0)
        {
            return Result.Err<string, EncoderError>(
                new EncoderError(EncoderErrorCode.InputNumberLessThanZero)
            );
        }

        return Result
            .Try(() => innerEncoder.Encode(source))
            .MapError(e => new EncoderError(EncoderErrorCode.InnerError, e));
    }

    public Result<long, EncoderError> Decode(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return Result.Err<long, EncoderError>(
                new EncoderError(EncoderErrorCode.InputStringIsEmpty)
            );
        }

        Result<IReadOnlyList<long>, Exception> decodeResult = Result.Try(() => innerEncoder.Decode(source));
        if (decodeResult.IsError)
        {
            EncoderError error = new(EncoderErrorCode.InnerError, decodeResult.Error);
            return Result.Err<long, EncoderError>(error);
        }

        if (decodeResult.Value is [var singleNumber])
        {
            return Result.Ok<long, EncoderError>(singleNumber);
        }

        return Result.Err<long, EncoderError>(
            new EncoderError(EncoderErrorCode.IncorrectInputString)
        );
    }
}