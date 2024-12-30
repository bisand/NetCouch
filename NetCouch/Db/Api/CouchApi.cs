using NetCouch.Db.Api.Elements;
using NetCouch.Http;

namespace NetCouch.Db.Api
{
    public class CouchApi(CouchDbClient requestClient, string defaultDatabase = "_users") : ICouchApi
    {
        protected readonly CouchDbClient _requestClient = requestClient;

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(_requestClient);
            return root;
        }

        public string DefaultDatabase { get; set; } = defaultDatabase;
    }
}