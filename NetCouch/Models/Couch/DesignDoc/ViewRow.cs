using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.DesignDoc
{
    [DataContract]
    public class ViewRow<T>
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "key")]
        public object Key { get; set; }

        [DataMember(Name = "value")]
        public object Value { get; set; }

        [DataMember(Name = "doc")]
        public T Doc { get; set; }
    }
}