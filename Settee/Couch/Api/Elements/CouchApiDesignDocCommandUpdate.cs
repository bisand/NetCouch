using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocCommandUpdate : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandUpdate(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}