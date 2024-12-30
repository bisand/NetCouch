using System.Text.Json.Serialization;
using NetCouch.Models.Couch.Database;

namespace NetCouch.Models.Couch;

public class CouchDoc : InternalResult
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("doc__type")]
    public string? DocType { get; set; }
}
