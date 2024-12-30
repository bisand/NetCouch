using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDoc : CouchApiDb
    {
        public CouchApiDesignDoc(CouchDbClient requestClient)
            : base(requestClient)
        {
        }
    }
}