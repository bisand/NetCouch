using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class RevInfo
{
    [JsonPropertyName("rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}