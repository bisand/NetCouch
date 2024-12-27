using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class DesignDoc
{
    public DesignDoc()
    {
        Views = [];
    }

    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("views")]
    public Dictionary<string, View>? Views { get; set; }
}
