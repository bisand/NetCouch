using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandShow(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}