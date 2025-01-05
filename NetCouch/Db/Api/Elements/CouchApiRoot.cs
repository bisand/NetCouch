using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiRoot(IRequestClient requestClient)
{
    protected internal readonly IRequestClient _requestClient = requestClient;

    public string PathElement { get; set; } = "/";
}