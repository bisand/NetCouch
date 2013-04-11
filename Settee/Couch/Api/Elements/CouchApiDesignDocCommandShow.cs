using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandShow(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}