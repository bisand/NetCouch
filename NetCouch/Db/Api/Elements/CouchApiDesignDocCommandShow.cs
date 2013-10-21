using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDesignDocCommandShow : CouchApiDesignDoc
    {
        public CouchApiDesignDocCommandShow(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}