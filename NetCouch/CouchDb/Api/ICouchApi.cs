using Biseth.Net.Settee.CouchDb.Api.Elements;

namespace Biseth.Net.Settee.CouchDb.Api
{
    public interface ICouchApi
    {
        CouchApiRoot Root();
        string DefaultDatabase { get; }
    }
}