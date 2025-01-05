using System;
using NetCouch.Models.Couch;

namespace NetCouchTests
{
    public class Person : CouchDoc
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int PersonType { get; set; }
    }
}