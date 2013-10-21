using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommand : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}