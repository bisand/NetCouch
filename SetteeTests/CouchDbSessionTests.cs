using System.Linq;
using Biseth.Net.Settee;
using NUnit.Framework;

namespace SetteeTests
{
    [TestFixture]
    public class CouchDbSessionTests
    {
        [Test]
        public void OpenSessionAndQueryTheDatabase()
        {
            using (var database = new CouchDatabase("http://localhost:5984/"))
            {
                using (var session = database.OpenSession("trivial"))
                {
                    var queryable = session.Query<Car>().Where(x => x.HorsePowers == 1337);
                    var cars = queryable.ToList();
                    Assert.That(cars != null && cars.Count > 0);
                }
            }
        }

        public void OpenSessionAndQueryTheDatabaseWithLinq()
        {
            using (var database = new CouchDatabase("http://localhost:5984/"))
            {
                using (var session = database.OpenSession("trivial"))
                {
                    var queryable = from car in session.Query<Car>()
                                    where car.HorsePowers == 1337
                                    select car;
                    var cars = queryable.ToList();
                    Assert.That(cars != null && cars.Count > 0);
                }
            }
        }

        [Test]
        public void WhenQueryingForFirstRecord_ThenOneRecordShouldBeReturned()
        {
            using (var database = new CouchDatabase("http://localhost:5984/"))
            {
                using (var session = database.OpenSession("trivial"))
                {
                    var car = session.Query<Car>().Where(x => x.HorsePowers == 1337).FirstOrDefault();
                    Assert.That(car != null);
                }
            }
        }
    }
}