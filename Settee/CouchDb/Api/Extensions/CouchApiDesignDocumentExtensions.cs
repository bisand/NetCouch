using Biseth.Net.Settee.CouchDb.Api.Elements;

namespace Biseth.Net.Settee.CouchDb.Api.Extensions
{
    public static class CouchApiDesignDocumentExtensions
    {
        public static CouchApiDesignDoc DesignDoc(this CouchApiDb element, string name = null)
        {
            var result = new CouchApiDesignDoc(element.RequestClient)
                {
                    PathElement = element.PathElement + "_design/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocAttachment Attachment(this CouchApiDesignDoc element, string attachment = null)
        {
            var result = new CouchApiDesignDocAttachment(element.RequestClient)
                {
                    PathElement = element.PathElement +
                                  (!string.IsNullOrWhiteSpace(attachment) ? (attachment + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommand Info(this CouchApiDesignDoc element)
        {
            var result = new CouchApiDesignDocCommand(element.RequestClient)
                {
                    PathElement = element.PathElement + "_info/"
                };
            return result;
        }

        public static CouchApiDesignDocCommandView View(this CouchApiDesignDoc element, string name, string parameters = null)
        {
            var result = new CouchApiDesignDocCommandView(element.RequestClient)
                {
                    PathElement = element.PathElement + "_view/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(parameters) ? ("?" + parameters + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommandShow Show(this CouchApiDesignDoc element, string name)
        {
            var result = new CouchApiDesignDocCommandShow(element.RequestClient)
                {
                    PathElement = element.PathElement + "_show/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommandList List(this CouchApiDesignDoc element, string name, string viewName)
        {
            var result = new CouchApiDesignDocCommandList(element.RequestClient)
                {
                    PathElement = element.PathElement + "_list/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(name) ? (viewName + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommandList List(this CouchApiDesignDoc element, string name, string otherDesignDoc, string viewName)
        {
            var result = new CouchApiDesignDocCommandList(element.RequestClient)
                {
                    PathElement = element.PathElement + "_list/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(otherDesignDoc) ? (otherDesignDoc + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(viewName) ? (viewName + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommandUpdate Update(this CouchApiDesignDoc element, string name, string doc = null)
        {
            var result = new CouchApiDesignDocCommandUpdate(element.RequestClient)
                {
                    PathElement = element.PathElement + "_update/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(doc) ? (doc + "/") : "")
                };
            return result;
        }

        public static CouchApiDesignDocCommandRewrite Rewrite(this CouchApiDesignDoc element, string name, string anything)
        {
            var result = new CouchApiDesignDocCommandRewrite(element.RequestClient)
                {
                    PathElement = element.PathElement + "_rewrite/" +
                                  (!string.IsNullOrWhiteSpace(name) ? (name + "/") : "") +
                                  (!string.IsNullOrWhiteSpace(anything) ? (anything + "/") : "")
                };
            return result;
        }
    }
}