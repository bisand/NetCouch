using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.Doc
{
    [DataContract]
    public class DocResponse
    {
        [DataMember(Name = "ok", EmitDefaultValue = false)]
        public string Ok { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(Name = "rev", EmitDefaultValue = false)]
        public string Rev { get; set; }

        [DataMember(Name = "error", EmitDefaultValue = false)]
        public string Error { get; set; }

        [DataMember(Name = "reason", EmitDefaultValue = false)]
        public string Reason { get; set; }
    }
}