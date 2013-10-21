using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
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