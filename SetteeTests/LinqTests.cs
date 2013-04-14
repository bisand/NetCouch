using System.Linq;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Linq;
using NUnit.Framework;

namespace SetteeTests
{
    [TestFixture]
    public class LinqTests
    {
        [Test]
        public void TestingSomeLinq()
        {
            var client = new RequestClient("http://localhost:5984/");
            var api = new CouchApi(client, "trivial");

            var query = new CouchDbQuery<Person>(new CouchDbQueryProvider<Person>(api));
            var persons = query.Where(p => p.FirstName == "André" && p.LastName == "Biseth" && p.Weight == 78).ToList();
            Assert.That(persons != null);
        }
    }
}