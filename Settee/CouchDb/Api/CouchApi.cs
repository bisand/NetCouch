using Biseth.Net.Settee.CouchDb.Api.Elements;
using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api
{
    public class CouchApi : ICouchApi
    {
        protected readonly IRequestClient RequestClient;

        public CouchApi(IRequestClient requestClient)
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