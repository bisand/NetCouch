using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommandUpdate : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandUpdate(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}