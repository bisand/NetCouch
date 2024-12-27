using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc
{
    
    public class ViewResponse<T>
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("total_rows")]
        public int TotalRows { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("rows")]
        public List<ViewRow<T>>? Rows { get; set; }
    }
}