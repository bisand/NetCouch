using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database
{
    public class Sizes
    {
        [JsonPropertyName("file")]
        public int File { get; set; }

        [JsonPropertyName("external")]
        public int External { get; set; }

        [JsonPropertyName("active")]
        public int Active { get; set; }
    }
}