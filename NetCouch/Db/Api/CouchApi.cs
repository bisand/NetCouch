using NetCouch.Db.Api.Elements;
using NetCouch.Http;

namespace NetCouch.Db.Api
{
    public class CouchApi(RequestClient requestClient, string defaultDatabase = "_users") : ICouchApi
    {
        protected readonly RequestClient _requestClient = requestClient;

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(_requestClient);
            return root;
        }

        public string DefaultDatabase { get; set; } = defaultDatabase;
    }
}