using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommand : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}