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

            var query = new CouchDbQuery<Car>(new CouchDbQueryProvider<Car>(api));
            var persons = query.Where(p => (p.Make == "Saab" && p.Model == "1337" && p.Make != "Volvo" || p.Model == "2013")).ToList();
            Assert.That(persons != null);
        }
    }
}