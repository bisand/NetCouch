using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiRootCommand : CouchApiDesignDoc
    {
        public CouchApiRootCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}