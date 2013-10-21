using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandUpdate : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandUpdate(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}