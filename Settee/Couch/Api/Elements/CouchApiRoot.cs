using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiRoot
    {
        protected internal readonly RequestClient RequestClient;

        public CouchApiRoot(RequestClient requestClient)
        {
            RequestClient = requestClient;
            PathElement = "/";
        }

        public string PathElement { get; set; }
    }
}