using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Settee.Models.Couch.Doc
{
    [DataContract]
    public class BulkDocsResponse : List<DocResponse>
    {
    }
}