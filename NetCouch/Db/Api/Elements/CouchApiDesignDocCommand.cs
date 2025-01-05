using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocCommand : CouchApiDesignDoc
{
    public CouchApiDesignDocCommand(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}