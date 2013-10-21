using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiConfig : CouchApiRoot
    {
        public CouchApiConfig(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}