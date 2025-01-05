using System.Text.Json.Serialization;
using NetCouch.Models.Couch.Database;

namespace NetCouch.Models.Couch;

public class CouchDocWrapper<T>(T document): CouchDbResult
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("doc_type_")]
    public string? DocType { get; set; }

    [JsonPropertyName("doc_")]
    public T Doc { get; set; } = document;
}