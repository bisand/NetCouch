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
            var api = new CouchApi(client);

            var query = new CouchDbQuery<Person>(api);
            var persons = (from p in query
                            where p.LastName == "Biseth"
                            select p).ToList();
            Assert.That(persons != null);
        }
    }
}