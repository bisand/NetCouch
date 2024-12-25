using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiRoot
    {
        protected internal readonly RequestClient _requestClient;

        public CouchApiRoot(RequestClient requestClient)
        {
            _requestClient = requestClient;
            PathElement = "/";
        }

        public string PathElement { get; set; }
    }
}