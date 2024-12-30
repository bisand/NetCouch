using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Doc;

public class DocResponse
{
    [JsonPropertyName("ok")]
    public bool? Ok { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}