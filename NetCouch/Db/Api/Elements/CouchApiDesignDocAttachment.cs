using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDesignDocAttachment : CouchApiDesignDoc
    {
        public CouchApiDesignDocAttachment(CouchDbClient requestClient)
            : base(requestClient)
        {
        }
    }
}