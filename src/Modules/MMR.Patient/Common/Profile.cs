using System.Text.Json.Serialization;
using MMR.Common.Encoding;
using MMR.Common.Enums;

namespace MMR.Patient.Common;

public class Profile
{
    [JsonConverter(typeof(EncodedLongJsonConverter))]
    public long Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Sex? Sex { get; set; }
}