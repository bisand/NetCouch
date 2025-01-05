using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiConfigSectionKey : CouchApiRoot
{
    public CouchApiConfigSectionKey(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}