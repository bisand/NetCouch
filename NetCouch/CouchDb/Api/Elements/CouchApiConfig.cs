using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiConfig : CouchApiRoot
    {
        public CouchApiConfig(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}