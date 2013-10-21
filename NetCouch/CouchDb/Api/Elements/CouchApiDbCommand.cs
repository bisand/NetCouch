using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.CouchDb.Api.Elements
{
    public class CouchApiDbCommand : CouchApiDb
    {
        public CouchApiDbCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}