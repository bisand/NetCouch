using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class Revisions
{
    [JsonPropertyName("ids")]
    public string[]? Ids { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }
}
