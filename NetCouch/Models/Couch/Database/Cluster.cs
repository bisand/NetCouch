using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database;

public class Cluster
{
    [JsonPropertyName("q")]
    public int Q { get; set; }

    [JsonPropertyName("n")]
    public int N { get; set; }

    [JsonPropertyName("w")]
    public int W { get; set; }

    [JsonPropertyName("r")]
    public int R { get; set; }
}