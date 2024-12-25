using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDb : CouchApiRoot
    {
        public CouchApiDb(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}