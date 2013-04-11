using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocCommandView : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandView(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}