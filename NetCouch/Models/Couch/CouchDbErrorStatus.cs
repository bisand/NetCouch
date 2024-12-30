using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch;

public class CouchDbErrorStatus
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}