using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocCommandList : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandList(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}