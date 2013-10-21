using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiConfigSection : CouchApiRoot
    {
        public CouchApiConfigSection(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}