using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.Doc
{
    [DataContract]
    public class BulkDocsRequest
    {
        public BulkDocsRequest(IEnumerable<object> docmuents)
        {
            Docs = new List<object>(docmuents);
        }

        [DataMember(Name = "docs", IsRequired = true)]
        public List<object> Docs { get; set; }
    }
}