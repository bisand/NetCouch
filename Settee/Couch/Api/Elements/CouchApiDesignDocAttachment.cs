using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDocAttachment : CouchApiDesignDoc
    {
        public CouchApiDesignDocAttachment(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}