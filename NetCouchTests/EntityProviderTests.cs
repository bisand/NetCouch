using System.Linq;
using NetCouch;
using NetCouch.IQToolkit;
using NetCouch.IQToolkit.Data.Common;
using NetCouch.IQToolkit.Data.Mapping;
using NetCouch.IQToolkit.Data.Providers.CouchDb;
using NetCouch.Models.Couch.DesignDoc;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class EntityProviderTests
    {
        private CouchDatabase? _database;

        [SetUp]
        public void Init()
        {
            DotEnv.Load(".env");
            var username = Environment.GetEnvironmentVariable("COUCHDB_USERNAME") ?? "";
            var password = Environment.GetEnvironmentVariable("COUCHDB_PASSWORD") ?? "";
            var url = Environment.GetEnvironmentVariable("COUCHDB_URL") ?? "";

            _database = new CouchDatabase(url, username, password);

            using var session = _database.OpenSession("trivial");
            var car = new Car { Id = Guid.NewGuid().ToString(), HorsePowers = 123, Make = "Audi", Model = "Test" };
            var designDoc = new DesignDoc();
            session.Store(car);
            session.SaveChanges();
        }

        [Test]
        public void TestNewInstance()
        {
            if (_database == null)
            {
                Assert.Fail("Database is null");
                return;
            }
            var mapping = new ImplicitMapping();
            var policy = new QueryPolicy();

            var provider = new CouchDbQueryProvider(new CouchDbConnection(_database), mapping, policy);
            var query = new Query<Car>(provider);
            var queryable = query.Where(x => x.Make == "Audi" && x.HorsePowers == 123);
            var cars = queryable.ToList();
            foreach (var car in cars)
            {
                car.Model = "Cool";
            }
        }
    }
}