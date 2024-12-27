using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Doc
{
    public class BulkDocsRequest
    {
        public BulkDocsRequest(IEnumerable<object> docmuents)
        {
            Docs = new List<object>(docmuents);
        }

        [JsonPropertyName("docs")]
        public List<object> Docs { get; set; }
    }
}