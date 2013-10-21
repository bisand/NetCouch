using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiRoot
    {
        protected internal readonly IRequestClient RequestClient;

        public CouchApiRoot(IRequestClient requestClient)
        {
            RequestClient = requestClient;
            PathElement = "/";
        }

        public string PathElement { get; set; }
    }
}