using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiConfigSection : CouchApiRoot
    {
        public CouchApiConfigSection(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}