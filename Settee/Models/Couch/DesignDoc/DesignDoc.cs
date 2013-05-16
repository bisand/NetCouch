using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Settee.Models.Couch.DesignDoc
{
    [DataContract]
    public class DesignDoc
    {
        public DesignDoc()
        {
            Views = new Dictionary<string, View>();
        }

        [DataMember(Name = "_id", EmitDefaultValue = false)]
        public string Id { get; set; }
        [DataMember(Name = "_rev", EmitDefaultValue = false)]
        public string Rev { get; set; }
        [DataMember(Name = "language", EmitDefaultValue = false)]
        public string Language { get; set; }
        [DataMember(Name = "views")]
        public Dictionary<string, View> Views { get; set; }
    }

    [DataContract]
    public class View
    {
        [DataMember(Name = "map", IsRequired = true)]
        public string Map { get; set; }
        [DataMember(Name = "reduce", EmitDefaultValue = false)]
        public string Reduce { get; set; }
    }
}