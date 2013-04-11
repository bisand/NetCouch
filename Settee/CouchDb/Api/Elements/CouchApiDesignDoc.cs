using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDoc : CouchApiDb
    {
        public CouchApiDesignDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}