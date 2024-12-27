using System;
using System.Collections.Generic;
using System.Diagnostics;
using NetCouch;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Models.Couch.Doc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class LinqTests
    {
        private string? _username;
        private string? _password;
        private string? _url;

        [SetUp]
        public void Init()
        {
            DotEnv.Load(".env");
            _username = Environment.GetEnvironmentVariable("COUCHDB_USERNAME") ?? "";
            _password = Environment.GetEnvironmentVariable("COUCHDB_PASSWORD") ?? "";
            _url = Environment.GetEnvironmentVariable("COUCHDB_URL") ?? "";
        }

        [Test]
        public void TestingSomeLinq()
        {
            var client = new RequestClient(_url, _username, _password);
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