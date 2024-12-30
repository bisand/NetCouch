using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiRoot(CouchDbClient requestClient)
{
    protected internal readonly CouchDbClient _requestClient = requestClient;

    public string PathElement { get; set; } = "/";
}