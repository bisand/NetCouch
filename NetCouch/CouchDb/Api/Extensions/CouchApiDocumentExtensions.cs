using Biseth.Net.Settee.CouchDb.Api.Elements;

namespace Biseth.Net.Settee.CouchDb.Api.Extensions
{
    public static class CouchApiDocumentExtensions
    {
        public static CouchApiDbCommand Doc(this CouchApiDb element, string documentId = null)
        {
            var result = new CouchApiDbCommand(element.RequestClient)
                {
                    PathElement = element.PathElement + (!string.IsNullOrWhiteSpace(documentId) ? (documentId + "/") : "")
                };
            return result;
        }

        public static CouchApiAttachment Attachment(this CouchApiDoc element, string attachment = null)
        {
            var result = new CouchApiAttachment(element.RequestClient)
                {
                    PathElement = element.PathElement + (!string.IsNullOrWhiteSpace(attachment) ? (attachment + "/") : "")
                };
            return result;
        }
    }
}