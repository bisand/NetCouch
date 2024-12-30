using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiAttachment : CouchApiDoc
{
    public CouchApiAttachment(CouchDbClient requestClient)
        : base(requestClient)
    {
    }
}