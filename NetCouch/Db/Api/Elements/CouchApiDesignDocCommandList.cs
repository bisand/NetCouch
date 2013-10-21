using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandList : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandList(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}