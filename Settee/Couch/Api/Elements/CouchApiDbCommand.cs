using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDbCommand : CouchApiDb
    {
        public CouchApiDbCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}