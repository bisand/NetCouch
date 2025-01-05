using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiRootCommand : CouchApiDesignDoc
{
    public CouchApiRootCommand(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}