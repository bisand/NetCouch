using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class View
{
    [JsonPropertyName("map")]
    public string? Map { get; set; }

    [JsonPropertyName("reduce")]
    public string? Reduce { get; set; }
}