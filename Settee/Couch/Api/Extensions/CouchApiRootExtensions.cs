using Biseth.Net.Settee.Couch.Api.Elements;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Models.Couch.Database;

namespace Biseth.Net.Settee.Couch.Api.Extensions
{
    public static class CouchApiRootExtensions
    {
        public static ResponseData<HttpGetRoot> Get(this CouchApiRoot element)
        {
            var responseData = element.RequestClient.Get<HttpGetRoot>(element.PathElement);
            return responseData;
        }
    }
}