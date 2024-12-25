using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiDoc : CouchApiDb
    {
        public CouchApiDoc(RequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}