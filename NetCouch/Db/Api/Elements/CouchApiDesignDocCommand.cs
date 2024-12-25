using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocCommand : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommand(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}