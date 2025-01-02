using NetCouch.Db.Api.Elements;

namespace NetCouch.Db.Api.Extensions;

public static class CouchApiDesignDocCommandViewExtensions
{
    public static CouchApiDesignDocCommandView Skip(this CouchApiDesignDocCommandView element, int count)
    {
        element.PathElement += "&skip=" + count;
        return element;
    }

    public static CouchApiDesignDocCommandView Limit(this CouchApiDesignDocCommandView element, int count)
    {
        element.PathElement += "&limit=" + count;
        return element;
    }
}