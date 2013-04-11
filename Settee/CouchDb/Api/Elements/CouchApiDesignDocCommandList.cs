using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommandList : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandList(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}