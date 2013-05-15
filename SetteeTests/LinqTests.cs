using System;
using System.Diagnostics;
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

            var query = new CouchDbQuery<Car>(new CouchDbQueryProvider<Car>(api, new CouchDbTranslation()));
            //var cars = query.Where(p => (((p.Make == "Saab" || (p.Model == "1337" && p.HorsePowers == 200)) || p.Make != "Volvo") && p.Model != "2013")).ToList();
            //var cars = query.Where(p => (p.Make == "Saab" && (p.Model == "1337" || p.HorsePowers == 1337))).ToList();
            Console.WriteLine("Starting to process queries...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                var cars = query.Where(p => p.Model == "1337").ToList();
                Assert.That(cars != null && cars.Count > 0);
            }
            stopwatch.Stop();
            Console.WriteLine("Finished!");
            Console.WriteLine("Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }
    }
}