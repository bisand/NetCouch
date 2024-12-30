using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandUpdate : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandUpdate(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}