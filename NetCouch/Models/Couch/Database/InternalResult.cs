using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database;

public class InternalResult
{
    [JsonPropertyName("ok")]
    public bool? Ok { get; private set; }

    [JsonPropertyName("error")]
    public string? Error { get; private set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; private set; }
}
