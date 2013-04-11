using Biseth.Net.Settee.Couch.Api.Elements;

namespace Biseth.Net.Settee.Couch.Api.Extensions
{
    public static class CouchApiDocumentExtensions
    {
        public static CouchApiDbCommand Doc(this CouchApiDb element, string documentId = null)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + (!string.IsNullOrWhiteSpace(documentId) ? (documentId + "/") : "")
            };
            return result;
        }

    }
}