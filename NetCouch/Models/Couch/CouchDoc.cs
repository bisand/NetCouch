using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch;

public class CouchDoc
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("doc__type")]
    public string? DocType { get; set; }
}
