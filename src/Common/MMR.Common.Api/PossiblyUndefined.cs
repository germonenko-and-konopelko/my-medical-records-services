using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMR.Common.Api;

public struct PossiblyUndefined<T>
{
    public readonly bool Defined = false;

    public readonly T? Value = default;

    public PossiblyUndefined(T? value)
    {
        Defined = true;
        Value = value;
    }
}

public class PossiblyUndefinedJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        return typeToConvert.GetGenericTypeDefinition() == typeof(PossiblyUndefined<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type valueType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(PossiblyUndefinedJsonConverter<>).MakeGenericType(valueType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
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