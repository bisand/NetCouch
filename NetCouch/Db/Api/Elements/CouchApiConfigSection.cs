using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiConfigSection : CouchApiRoot
{
    public CouchApiConfigSection(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}