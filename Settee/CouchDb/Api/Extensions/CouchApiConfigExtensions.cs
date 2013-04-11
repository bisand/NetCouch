using Biseth.Net.Settee.CouchDb.Api.Elements;

namespace Biseth.Net.Settee.CouchDb.Api.Extensions
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