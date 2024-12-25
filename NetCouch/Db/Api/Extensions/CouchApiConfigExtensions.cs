using NetCouch.Db.Api.Elements;

namespace NetCouch.Db.Api.Extensions
{
    public static class CouchApiConfigExtensions
    {
        public static CouchApiConfig Config(this CouchApiRoot element)
        {
            var result = new CouchApiConfig(element._requestClient);
            result.PathElement = element.PathElement + "_config/";
            return result;
        }

        public static CouchApiConfigSection Section(this CouchApiConfig element, string section)
        {
            var result = new CouchApiConfigSection(element._requestClient);
            result.PathElement = element.PathElement + section + "/";
            return result;
        }

        public static CouchApiConfigSectionKey Key(this CouchApiConfigSection element, string key)
        {
            var result = new CouchApiConfigSectionKey(element._requestClient);
            result.PathElement = element.PathElement + key + "/";
            return result;
        }
    }
}