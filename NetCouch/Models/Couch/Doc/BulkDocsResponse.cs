using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.Doc
{
    [DataContract]
    public class BulkDocsResponse : List<DocResponse>
    {
    }
}