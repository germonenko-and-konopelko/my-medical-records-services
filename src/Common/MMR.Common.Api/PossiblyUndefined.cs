using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMR.Common.Api;

public struct PossiblyUndefined<T>
{
    public readonly bool Defined;

    public readonly T? Value;

    public PossiblyUndefined()
    {
        Defined = false;
        Value = default;
    }

    public PossiblyUndefined(T? value)
    {
        Defined = true;
        Value = value;
    }
}

public class PossiblyUndefinedJsonConverter<T> : JsonConverter<PossiblyUndefined<T>>
{
    public override PossiblyUndefined<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var innerValue = JsonSerializer.Deserialize<T>(ref reader, options);
        return new PossiblyUndefined<T>(innerValue);
    }

    public override void Write(Utf8JsonWriter writer, PossiblyUndefined<T> value, JsonSerializerOptions options)
    {
        if (value.Defined)
        {
            writer.WriteRawValue(JsonSerializer.Serialize(value.Value, options));
        }
    }
}