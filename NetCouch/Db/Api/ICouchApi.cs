using Biseth.Net.Couch.Db.Api.Elements;

namespace Biseth.Net.Couch.Db.Api
{
    public interface ICouchApi
    {
        string DefaultDatabase { get; }
        CouchApiRoot Root();
    }
}