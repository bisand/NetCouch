using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandRewrite : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandRewrite(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}