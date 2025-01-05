using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDoc : CouchApiDb
{
    public CouchApiDoc(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}