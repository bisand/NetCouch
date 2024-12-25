using NetCouch.Db.Api.Elements;

namespace NetCouch.Db.Api.Extensions
{
    public static class CouchApiDocumentExtensions
    {
        public static CouchApiDbCommand Doc(this CouchApiDb element, string documentId = null)
        {
            var result = new CouchApiDbCommand(element._requestClient)
                {
                    PathElement = element.PathElement + (!string.IsNullOrWhiteSpace(documentId) ? (documentId + "/") : "")
                };
            return result;
        }

        public static CouchApiAttachment Attachment(this CouchApiDoc element, string attachment = null)
        {
            var result = new CouchApiAttachment(element._requestClient)
                {
                    PathElement = element.PathElement + (!string.IsNullOrWhiteSpace(attachment) ? (attachment + "/") : "")
                };
            return result;
        }
    }
}