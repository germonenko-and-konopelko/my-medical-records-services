using System.Text.Json;
using System.Text.Json.Serialization;
using MMR.Common.Results;

namespace MMR.Common.Encoding;

public class EncodedLongJsonConverter(IEncoder encoder) : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(long))
        {
            return 0;
        }

        var jsonValue = reader.GetString();
        if (jsonValue == null)
        {
            return 0;
        }

        Result<long, EncoderError> decodeResult = encoder.Decode(jsonValue);
        return decodeResult.IsOk ? decodeResult.Value : 0;
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        encoder.Encode(value).Inspect(
            writer.WriteStringValue,
            _ => writer.WriteStringValue(string.Empty));
    }
}