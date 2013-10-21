namespace Biseth.Net.Settee.Models.Couch.Database
{
    public class HttpGetRoot
    {
        public string CouchDb { get; set; }
        public string Uuid { get; set; }
        public string Version { get; set; }
        public DatabaseVendor Vendor { get; set; }
    }
}