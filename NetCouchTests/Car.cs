
using NetCouch.Models.Couch;

namespace NetCouchTests
{
    public class Car : CouchDoc
    {
        public int HorsePowers { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
    }
}