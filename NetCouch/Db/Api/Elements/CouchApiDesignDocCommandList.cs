using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandList : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandList(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}