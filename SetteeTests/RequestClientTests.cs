using System;
using System.Net;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Models.Couch.Database;
using NUnit.Framework;

namespace SetteeTests
{
    [TestFixture]
    public class RequestClientTests
    {
        [Test]
        public void When_getting_data_from_then_server__Then_it_should_be_deserialized()
        {
            var client = new RequestClient("http://localhost:5984/");
            var api = new CouchApi(client);

            var responseData = api.Root().Stats().Get<dynamic>();

            var rootData = api.Root().Get<HttpGetRoot>();
            Assert.IsNotNull(rootData);

            var configData = api.Root().Config().Get<dynamic>();
            Assert.IsNotNull(configData);

            var configSectionData = api.Root().Config().Section("daemons").Get<dynamic>();
            var indexServer = configSectionData.DataDeserialized.index_server.ToString();
            Assert.IsNotNull(configSectionData);

            var dbData = api.Root().Db("Test").Get<dynamic>();
            if (dbData.StatusCode != HttpStatusCode.OK)
            {
                var newDbData = api.Root().Db("Test").Put<dynamic, object>();
            }
            var person = new Person();
            person.FirstName = "André";
            person.LastName = "Biseth";
            person.BirthDate = new DateTime(1974, 3, 12);
            person.Weight = 78;
            person.Height = 178;

            var post = api.Root().Db("test").Doc().Post<Person, dynamic>(person);
            Assert.IsNotNull(post);

            var postDoc = api.Root().Db("test").Doc("Test").Put<Person, dynamic>(person);
            Assert.IsNotNull(postDoc);
        }
    }
}