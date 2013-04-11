using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDoc : CouchApiDb
    {
        public CouchApiDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}