using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Biseth.Net.Settee.Models.Couch.DesignDoc
{
    [DataContract]
    public class DesignDoc
    {
        [DataMember(Name = "_id")]
        public string Id { get; set; }
        [DataMember(Name = "_rev")]
        public string Rev { get; set; }
        [DataMember(Name = "views")]
        public Dictionary<string, View> Views { get; set; }
    }

    public class View
    {
        [DataMember(Name = "map", IsRequired = true)]
        public string Map { get; set; }
        [DataMember(Name = "reduce")]
        public string Reduce { get; set; }
    }
}