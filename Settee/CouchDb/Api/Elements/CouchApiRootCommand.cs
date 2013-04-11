using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiRootCommand : CouchApiDesignDoc
    {
        public CouchApiRootCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}