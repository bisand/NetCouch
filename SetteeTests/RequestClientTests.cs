using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Models.Couch;
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
            var data = client.Get<HttpGetRoot>("/");
            Assert.IsNotNull(data);
        }
    }
}
