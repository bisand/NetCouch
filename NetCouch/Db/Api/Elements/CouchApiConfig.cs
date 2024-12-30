using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiConfig : CouchApiRoot
    {
        public CouchApiConfig(CouchDbClient requestClient)
            : base(requestClient)
        {
        }
    }
}