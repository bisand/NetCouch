using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandShow(CouchDbClient requestClient)
            : base(requestClient)
        {
        }
    }
}