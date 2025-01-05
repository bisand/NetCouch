using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiConfig : CouchApiRoot
{
    public CouchApiConfig(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}