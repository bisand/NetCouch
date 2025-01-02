using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NetCouch;
using NetCouch.Models.Couch.DesignDoc;
using NetCouch.Models.Couch.Doc;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class CouchDbSessionTests
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
            var car = new Car { Id = Guid.NewGuid().ToString(), HorsePowers = 1337, Make = "Tesla", Model = "Model 3" };
            session.Store(car);
            session.SaveChanges();
        }

        [Test]
        public void OpenSessionAndQueryTheDatabase()
        {
            if (_database == null)
            {
                Assert.Fail("Database is null");
                return;
            }
            using var session = _database.OpenSession("trivial");
            var queryable = session.Query<Car>().Where(x => x.HorsePowers == 1337 && x.Make == "Tesla");
            var cars = queryable.ToList();
            foreach (var car in cars)
            {
                car.Model = "Model Y";
                var test = session.Load<Car>(car.Id);
            }
            session.SaveChanges();
            Assert.That(cars != null && cars.Any());
        }

        [Test]
        public void CreateAndStoreManyObjectsInsideSession()
        {
            if (_database == null)
            {
                Assert.Fail("Database is null");
                return;
            }
            //for (int t = 0; t < 10; t++)
            var swTotal = new Stopwatch();
            swTotal.Start();
            Parallel.For(0, 10, t =>
            {
                var sw = new Stopwatch();
                sw.Start();
                using var session = _database.OpenSession("trivial");
                for (var i = 0; i < 10000; i++)
                {
                    var car = new Car { Id = Guid.NewGuid().ToString(), HorsePowers = 100 + i, Make = "Tesla", Model = $"Model {i}" };
                    session.Store(car);
                }
                session.SaveChanges();
                sw.Stop();
                Console.WriteLine("{0} - Elapsed: {1} ms.", t, sw.ElapsedMilliseconds);
            });
            swTotal.Stop();
            Console.WriteLine("Total - Elapsed: {0} ms.", swTotal.ElapsedMilliseconds);
        }

        [Test]
        public void OpenSessionAndQueryTheDatabaseWithLinq()
        {
            if (_database == null)
            {
                Assert.Fail("Database is null");
                return;
            }
            using var session = _database.OpenSession("trivial");
            var queryable = from car in session.Query<Car>()
                            where car.HorsePowers == 1337
                            select car;
            var cars = queryable.Skip(1).Take(1).ToList();
            Assert.That(cars != null && cars.Count > 0);
        }

        [Test]
        public void WhenQueryingForFirstRecord_ThenOneRecordShouldBeReturned()
        {
            if (_database == null)
            {
                Assert.Fail("Database is null");
                return;
            }
            using var session = _database.OpenSession("trivial");
            var car = session.Query<Car>().FirstOrDefault(x => x.HorsePowers == 1337);
            Assert.That(car != null);
        }
    }
}