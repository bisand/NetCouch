using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandShow(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}