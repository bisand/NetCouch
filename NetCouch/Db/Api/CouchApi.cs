using Biseth.Net.Couch.Db.Api.Elements;
using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api
{
    public class CouchApi : ICouchApi
    {
        protected readonly IRequestClient RequestClient;

        public CouchApi(IRequestClient requestClient, string defaultDatabase = "_users")
        {
            RequestClient = requestClient;
            DefaultDatabase = defaultDatabase;
        }

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(RequestClient);
            return root;
        }

        public string DefaultDatabase { get; set; }
    }
}