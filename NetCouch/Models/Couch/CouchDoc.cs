using System.Text.Json.Serialization;
using NetCouch.Models.Couch.Database;

namespace NetCouch.Models.Couch;

public class CouchDoc : CouchDbResult
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("doc_type_")]
    public string? DocType { get; set; }

    public CouchDoc() => DocType = GetType().Name;
}
