using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandRewrite : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandRewrite(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}