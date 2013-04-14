using Biseth.Net.Settee.CouchDb.Api.Elements;
using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api
{
    public class CouchApi : ICouchApi
    {
        protected readonly IRequestClient RequestClient;
        private readonly string _defaultDatabase;

        public CouchApi(IRequestClient requestClient, string defaultDatabase = "_users")
        {
            RequestClient = requestClient;
            _defaultDatabase = defaultDatabase;
        }

        public CouchApiRoot Root()
        {
            var root = new CouchApiRoot(RequestClient);
            return root;
        }

        public string DefaultDatabase
        {
            get { return _defaultDatabase; }
        }
    }
}