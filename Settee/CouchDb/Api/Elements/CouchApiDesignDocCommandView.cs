using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommandView : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandView(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}