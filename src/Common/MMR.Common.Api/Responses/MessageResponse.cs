using System.Text.Json.Serialization;

namespace MMR.Common.Api.Responses;

public class MessageResponse(string message)
{
    [JsonPropertyName("message")]
    public readonly string Message = message;
}