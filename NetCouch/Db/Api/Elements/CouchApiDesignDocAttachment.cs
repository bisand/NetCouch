using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocAttachment : CouchApiDesignDoc
    {
        public CouchApiDesignDocAttachment(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}