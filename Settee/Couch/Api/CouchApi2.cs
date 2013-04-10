namespace Biseth.Net.Settee.Couch.Helpers.Api
{
    public static class CouchApi2
    {
        public static string Root()
        {
            return "/";
        }

        public static string Config(this string str, string section)
        {
            str = str.TrimEnd(new[] { '/' });
            return string.Format("{0}/_config/{1}/", str, section);
        }

        public static string Config(this string str, string section, string key)
        {
            str = str.TrimEnd(new[] {'/'});
            return string.Format("{0}/_config/{1}/{2}/", str, section, key);
        }
    
    }
}