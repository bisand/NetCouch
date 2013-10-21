using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandView : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandView(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}