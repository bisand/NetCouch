using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandRewrite : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandRewrite(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}