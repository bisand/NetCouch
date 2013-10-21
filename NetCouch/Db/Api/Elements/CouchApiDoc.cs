using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDoc : CouchApiDb
    {
        public CouchApiDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}