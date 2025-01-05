using NetCouch.Http;

namespace NetCouch.Db.Api.Elements;

public class CouchApiDesignDocAttachment : CouchApiDesignDoc
{
    public CouchApiDesignDocAttachment(IRequestClient requestClient)
        : base(requestClient)
    {
    }
}