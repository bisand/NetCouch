using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database;

public class DatabaseVendor
{
    [JsonPropertyName("vendor")]
    public string? Version { get; set; }
    [JsonPropertyName("version")]
    public string? Name { get; set; }
}