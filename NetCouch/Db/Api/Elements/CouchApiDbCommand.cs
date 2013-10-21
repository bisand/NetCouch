using Biseth.Net.Couch.Http;

namespace Biseth.Net.Couch.Db.Api.Elements
{
    public class CouchApiDbCommand : CouchApiDb
    {
        public CouchApiDbCommand(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}