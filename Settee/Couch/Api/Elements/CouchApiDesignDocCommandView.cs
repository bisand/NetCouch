using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocCommandView : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandView(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}