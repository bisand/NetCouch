using NetCouch.Db.Api.Elements;
using NetCouch.Http;

namespace NetCouch.Db.Api
{
    public class CouchApi : ICouchApi
    {
        protected readonly RequestClient _requestClient;

        public CouchApi(RequestClient requestClient, string defaultDatabase = "_users")
        {
            _requestClient = requestClient;
            DefaultDatabase = defaultDatabase;
        }

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(_requestClient);
            return root;
        }

        public string DefaultDatabase { get; set; }
    }
}