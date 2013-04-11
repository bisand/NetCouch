using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDb : CouchApiRoot
    {
        public CouchApiDb(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}