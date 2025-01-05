using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandList : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandList(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}