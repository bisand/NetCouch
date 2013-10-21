using Biseth.Net.Couch.Db.Api.Elements;

namespace Biseth.Net.Couch.Db.Api.Extensions
{
    public static class CouchApiConfigExtensions
    {
        public static CouchApiConfig Config(this CouchApiRoot element)
        {
            var result = new CouchApiConfig(element.RequestClient);
            result.PathElement = element.PathElement + "_config/";
            return result;
        }

        public static CouchApiConfigSection Section(this CouchApiConfig element, string section)
        {
            var result = new CouchApiConfigSection(element.RequestClient);
            result.PathElement = element.PathElement + section + "/";
            return result;
        }

        public static CouchApiConfigSectionKey Key(this CouchApiConfigSection element, string key)
        {
            var result = new CouchApiConfigSectionKey(element.RequestClient);
            result.PathElement = element.PathElement + key + "/";
            return result;
        }
    }
}