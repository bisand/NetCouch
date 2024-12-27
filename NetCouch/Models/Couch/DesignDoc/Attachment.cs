using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class Attachment
{
    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }

    [JsonPropertyName("digest")]
    public string? Digest { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }
    
    [JsonPropertyName("revpos")]
    public int RevPos { get; set; }

    [JsonPropertyName("stub")]
    public bool Stub { get; set; }
}