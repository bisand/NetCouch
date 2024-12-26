using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiRoot(RequestClient requestClient)
    {
        protected internal readonly RequestClient _requestClient = requestClient;

        public string PathElement { get; set; } = "/";
    }
}