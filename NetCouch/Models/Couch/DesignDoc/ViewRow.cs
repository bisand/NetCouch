using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc
{
    
    public class ViewRow<T>
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("key")]
        public object? Key { get; set; }

        [JsonPropertyName("value")]
        public object? Value { get; set; }

        [JsonPropertyName("doc")]
        public T? Doc { get; set; }
    }
}