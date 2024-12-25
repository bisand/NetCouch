using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandList : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandList(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}