using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDb : CouchApiRoot
    {
        public CouchApiDb(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}