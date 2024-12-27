using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class DesignDoc : CouchDoc
{
    public DesignDoc()
    {
        Views = [];
    }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("views")]
    public Dictionary<string, View>? Views { get; set; }
}
