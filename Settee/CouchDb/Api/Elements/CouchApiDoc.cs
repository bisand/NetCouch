using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDoc : CouchApiDb
    {
        public CouchApiDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}