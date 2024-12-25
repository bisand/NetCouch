using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandView : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandView(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}