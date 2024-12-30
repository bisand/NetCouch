using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDbCommand : CouchApiDb
{
    public CouchApiDbCommand(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}