using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiAttachment : CouchApiDoc
    {
        public CouchApiAttachment(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}