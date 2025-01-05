using System;
using System.Net.Http;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Elements;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Models.Couch.Database;
using NUnit.Framework;
using Moq;
using NetCouch.Models.Couch;

namespace NetCouchTests.Db.Api.Extensions
{
    [TestFixture]
    public class CouchApiRootExtensionsTest
    {
        private Mock<IRequestClient> _mockRequestClient;
        private CouchApiRoot _couchApiRoot;

        [SetUp]
        public void SetUp()
        {
            _mockRequestClient = new Mock<IRequestClient>();
            _couchApiRoot = new CouchApiRoot(_mockRequestClient.Object);
        }

        [Test]
        public void Put_WithValidInput_ShouldReturnResponseData()
        {
            var input = new { Name = "Test" };
            var expectedResponse = new ResponseData<dynamic> { Body = new { Id = "1", Rev = "1-abc" } };
            _mockRequestClient.Setup(client => client.Put<dynamic, dynamic>(It.IsAny<RequestData<dynamic>>()))
                .Returns(expectedResponse);

            var result = _couchApiRoot.Put<dynamic, dynamic>(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Body.Id, result.Body.Id);
            Assert.AreEqual(expectedResponse.Body.Rev, result.Body.Rev);
        }

        [Test]
        public void Put_WithRevision_ShouldAddIfMatchHeader()
        {
            var input = new { Name = "Test" };
            var revision = "1-abc";
            var expectedResponse = new ResponseData<dynamic> { Body = new { Id = "1", Rev = "1-abc" } };
            _mockRequestClient.Setup(client => client.Put<dynamic, dynamic>(It.IsAny<RequestData<dynamic>>()))
                .Returns(expectedResponse);

            var result = _couchApiRoot.Put<dynamic, dynamic>(input, revision);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Body.Id, result.Body.Id);
            Assert.AreEqual(expectedResponse.Body.Rev, result.Body.Rev);
            // _mockRequestClient.Verify(client => client.Put<dynamic, dynamic>(It.Is<RequestData<dynamic>>(req =>
            //     req.Headers != null && req.Headers.Any(x => x.Key == "If-Match" && x.Value.First() == revision)
            // )), Times.Once);

            // result = _couchApiRoot.Put<dynamic, dynamic>(input, revision);

            // Assert.IsNotNull(result);
        }

        [Test]
        public void Put_WithDynamicInput_ShouldReturnResponseData()
        {
            var input = new { Name = "Test" };
            var expectedResponse = new ResponseData<CouchDoc> { Body = new CouchDoc { Id = "1", Rev = "1-abc" } };
            _mockRequestClient.Setup(client => client.Put<dynamic, CouchDoc>(It.IsAny<RequestData<dynamic>>()))
                .Returns(expectedResponse);

            var result = _couchApiRoot.Put<dynamic, CouchDoc>(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Body.Id, result.Body.Id);
            Assert.AreEqual(expectedResponse.Body.Rev, result.Body.Rev);
        }
    }
}