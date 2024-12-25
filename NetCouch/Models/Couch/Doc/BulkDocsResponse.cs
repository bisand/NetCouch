using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NetCouch.Models.Couch.Doc
{
    [DataContract]
    public class BulkDocsResponse : List<DocResponse>
    {
    }
}