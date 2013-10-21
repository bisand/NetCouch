using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDoc : CouchApiDb
    {
        public CouchApiDesignDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}