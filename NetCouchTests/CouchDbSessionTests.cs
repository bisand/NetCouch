using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Biseth.Net.Couch;
using Biseth.Net.Couch.Models.Couch.Doc;
using NUnit.Framework;

namespace NetCouchTests
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
                    //var queryable = session.Query<Car>().Where(x => (x.HorsePowers == 1337 || x.Model == "1337") && x.Make == "Saab");
                    var queryable = session.Query<Car>().Where(x => (x.HorsePowers == 123 && x.Make == "Audi"));
                    var cars = queryable.ToList();
                    foreach (var car in cars)
                    {
                        car.Model = "Cool";
                        var test = session.Load<Car>(car.Id);
                    }
                    session.SaveChanges();
                    Assert.That(cars != null && cars.Any());
                }
            }
        }

        [Test]
        public void CreateAndStoreManyObjectsInsideSession()
        {
            for (int t = 0; t < 10; t++)
            {
                var sw = new Stopwatch();
                sw.Start();
                using (var database = new CouchDatabase("http://localhost:5984/"))
                {
                    using (var session = database.OpenSession("trivial"))
                    {
                        for (var i = 0; i < 1000; i++)
                        {
                            var car = new Car {Id = Guid.NewGuid().ToString(), HorsePowers = 10 + i, Make = "Audi", Model = i.ToString()};
                            session.Store(car);
                        }
                        session.SaveChanges();
                    }
                }
                sw.Stop();
                Console.WriteLine("{0} - Elapsed: {1} ms.", t, sw.ElapsedMilliseconds);
            }
        }

        [Test]
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
                    var car = session.Query<Car>().FirstOrDefault(x => x.HorsePowers == 1337);
                    Assert.That(car != null);
                }
            }
        }
    }
}