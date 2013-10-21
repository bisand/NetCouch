using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiAttachment : CouchApiDoc
    {
        public CouchApiAttachment(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}