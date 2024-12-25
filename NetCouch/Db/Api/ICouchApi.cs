using NetCouch.Db.Api.Elements;

namespace NetCouch.Db.Api
{
    public interface ICouchApi
    {
        string DefaultDatabase { get; }
        CouchApiRoot Root();
    }
}