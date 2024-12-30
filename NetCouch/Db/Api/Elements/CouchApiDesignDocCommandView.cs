using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandView : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandView(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}