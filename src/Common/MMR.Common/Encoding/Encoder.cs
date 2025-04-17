using MMR.Common.Results;
using Sqids;

namespace MMR.Common.Encoding;

public class Encoder(SqidsEncoder<long> innerEncoder)
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

        var decodeResult = Result.Try(() => innerEncoder.Decode(source));
        if (decodeResult.IsError)
        {
            var error = new EncoderError(EncoderErrorCode.InnerError, decodeResult.Error);
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