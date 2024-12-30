using NetCouch.Http;

namespace NetCouch.Db.Api.Elements
{
    public class CouchApiRootCommand : CouchApiDesignDoc
    {
        public CouchApiRootCommand(CouchDbClient requestClient)
            : base(requestClient)
        {
        }
    }
}