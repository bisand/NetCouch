using System;
using System.Collections.Generic;
using System.Diagnostics;
using Biseth.Net.Couch;
using Biseth.Net.Couch.Db.Api;
using Biseth.Net.Couch.Db.Api.Extensions;
using Biseth.Net.Couch.Http;
using Biseth.Net.Couch.Models.Couch.Doc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class LinqTests
    {
        [Test]
        public void TestingSomeLinq()
        {
            var client = new RequestClient("http://localhost:5984/");
            var api = new CouchApi(client, "trivial");

            //var cars = query.Where(p => (((p.Make == "Saab" || (p.Model == "1337" && p.HorsePowers == 200)) || p.Make != "Volvo") && p.Model != "2013")).ToList();
            //var cars = query.Where(p => (p.Make == "Saab" && (p.Model == "1337" || p.HorsePowers == 1337))).ToList();
            Console.WriteLine("Starting to process queries...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var cars = new List<object>();
            for (var i = 0; i < 1000; i++)
            {
                var car = new Car {Id = Guid.NewGuid().ToString(), HorsePowers = 10 + i, Make = "Audi", Model = i.ToString()};
                dynamic obj = new CouchObjectProxy<Car>(car);
                obj.Test = "Test123";
                cars.Add(obj);
            }
            var request = new BulkDocsRequest(cars);
            var responseData = api.Root().Db("trivial").BulkDocs().Post<BulkDocsRequest, BulkDocsResponse>(request);

            stopwatch.Stop();
            Console.WriteLine("Finished!");
            Console.WriteLine("Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void CheckIfCouchObjectProxyReturnsSameEntityAsGiven()
        {
            Console.WriteLine("Starting to process queries...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var car = new Car { HorsePowers = 180, Make = "Audi", Model = "A6 Quatro" };
            dynamic obj = new CouchObjectProxy<Car>(car);
            for (int i = 0; i < 1000; i++)
            {
                obj.HorsePowers = i;
            }
            var originalEntity = obj.OriginalEntity;
            var entity = obj.Entity;
            Assert.AreEqual(car, originalEntity);
            Assert.AreNotEqual(car, entity);

            stopwatch.Stop();
            Console.WriteLine("Finished!");
            Console.WriteLine("Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }
    }
}