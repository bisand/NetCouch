using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiConfig : CouchApiRoot
    {
        public CouchApiConfig(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}