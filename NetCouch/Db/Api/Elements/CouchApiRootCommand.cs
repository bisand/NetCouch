using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiRootCommand : CouchApiDesignDoc
    {
        public CouchApiRootCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}