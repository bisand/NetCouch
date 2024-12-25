using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDoc : CouchApiDb
    {
        public CouchApiDesignDoc(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}