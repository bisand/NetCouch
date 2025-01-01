using System.Diagnostics;
using System.Text.Json;
using NetCouch;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Linq;
using NetCouch.Models.Couch.DesignDoc;
using NetCouch.Models.Couch.Doc;

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
        public void TestFirstOrDefault()
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_url))
            {
                Assert.Fail("Environment variables are not set");
                return;
            }
            var client = new CouchDbClient(_url, _username, _password);
            var api = new CouchApi(client, "trivial");
            var provider = new CouchDbQueryProvider<Car>(api, new CouchDbTranslation(), []);
            var queryable = new CouchDbQueryable<Car>(provider);

            var result = queryable.FirstOrDefault();

            // Assert the result
            Assert.IsNotNull(result);
        }

        [Test]
        public void Deserialize_ViewsProperty_ShouldBeDeserializedCorrectly()
        {
            // Arrange
            string jsonString = @"{
                ""_id"": ""_design/Car"",
                ""_rev"": ""16-3bd6e35abc1a0efaf67bd304d70858c8"",
                ""filters"": {},
                ""updates"": {},
                ""views"": {
                    ""HorsePowers"": {
                        ""map"": ""function(doc) { if (doc.doc_type_ && doc.doc_type_ == 'Car') { emit([doc.horsePowers],null); } }""
                    }
                },
                ""doc_type_"": ""DesignDoc""
            }";

            // Act
            var designDoc = JsonSerializer.Deserialize<DesignDoc>(jsonString);

            // Debug output
            Console.WriteLine(JsonSerializer.Serialize(designDoc));

            string map = "function(doc) { if (doc.doc_type_ && doc.doc_type_ == 'Car') { emit([doc.horsePowers],null); } }";

            // Assert
            Assert.NotNull(designDoc.Views);
            Assert.True(designDoc.Views.ContainsKey("HorsePowers"));
            Assert.That(map, Is.EqualTo(designDoc.Views["HorsePowers"].Map));
        }

        [Test]
        public void TestingSomeLinq()
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_url))
            {
                Assert.Fail("Environment variables are not set");
                return;
            }
            var client = new CouchDbClient(_url, _username, _password);
            var api = new CouchApi(client, "trivial");

            //var cars = query.Where(p => (((p.Make == "Saab" || (p.Model == "1337" && p.HorsePowers == 200)) || p.Make != "Volvo") && p.Model != "2013")).ToList();
            //var cars = query.Where(p => (p.Make == "Saab" && (p.Model == "1337" || p.HorsePowers == 1337))).ToList();
            Console.WriteLine("Starting to process queries...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var cars = new List<object>();
            for (var i = 0; i < 1000; i++)
            {
                var car = new Car { Id = Guid.NewGuid().ToString(), HorsePowers = 100 + i, Make = "Tesla", Model = $"Model {i + 1}" };
                cars.Add(car);
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

            var car = new Car { HorsePowers = 180, Make = "Tesla", Model = "Model 3" };
            for (int i = 0; i < 1000; i++)
            {
                car.HorsePowers = i;
            }

            stopwatch.Stop();
            Console.WriteLine("Finished!");
            Console.WriteLine("Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }
    }
}