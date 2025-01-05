using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
{
    public CouchApiDesignDocCommandShow(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}