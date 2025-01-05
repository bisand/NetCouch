using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database;

public class CouchDbResult
{
    [JsonPropertyName("ok")]
    public bool? Ok { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
