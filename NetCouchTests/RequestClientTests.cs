using System;
using System.Net;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Models.Couch.Database;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class RequestClientTests
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
        public void When_getting_data_from_then_server__Then_it_should_be_deserialized()
        {
            if (string.IsNullOrEmpty(_url) || string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                Assert.Fail("Missing environment variables");
            }
            var client = new RequestClient(_url, _username, _password);
            var api = new CouchApi(client);

            var responseData = api.Root().Stats().Get<dynamic>();

            var rootData = api.Root().Get<HttpGetRoot>();
            Assert.IsNotNull(rootData);

            var configData = api.Root().Config().Get<dynamic>();
            Assert.IsNotNull(configData);

            var configSectionData = api.Root().Config().Section("daemons").Get<dynamic>();
            var indexServer = configSectionData.Body.index_server.ToString();
            Assert.IsNotNull(configSectionData);

            var dbData = api.Root().Db("Test").Get<dynamic>();
            if (dbData.StatusCode != HttpStatusCode.OK)
            {
                var newDbData = api.Root().Db("Test").Put<dynamic, object>();
            }
            var person = new Person
            {
                FirstName = "André",
                LastName = "Biseth",
                BirthDate = new DateTime(1974, 3, 12),
                Weight = 80,
                Height = 180
            };

            var post = api.Root().Db("test").Doc().Post<Person, dynamic>(person);
            Assert.IsNotNull(post);

            var getDoc = api.Root().Db("test").Doc("Test").Get<Person>();
            Assert.IsNotNull(getDoc);

            getDoc.Body.Weight = 77;

            var postDoc = api.Root().Db("test").Doc("Test").Put<Person, dynamic>(getDoc.Body, (string)getDoc.DynamicData._rev);
            Assert.IsNotNull(postDoc);


        }
    }
}