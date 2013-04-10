using Biseth.Net.Settee.Couch.Api.Elements;
using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api
{
    public class CouchApi : ICouchApi
    {
        protected readonly RequestClient RequestClient;

        public CouchApi(RequestClient requestClient)
        {
            RequestClient = requestClient;
        }

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(RequestClient);
            return root;
        }
    }
}