using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommandRewrite : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandRewrite(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}