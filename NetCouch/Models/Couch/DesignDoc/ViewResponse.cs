using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.DesignDoc
{
    [DataContract]
    public class ViewResponse<T>
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "reason")]
        public string Reason { get; set; }

        [DataMember(Name = "total_rows")]
        public int TotalRows { get; set; }

        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        [DataMember(Name = "rows")]
        public List<ViewRow<T>> Rows { get; set; }
    }
}