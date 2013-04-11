using System;
using Biseth.Net.Settee.Couch.Api.Elements;

namespace Biseth.Net.Settee.Couch.Api.Extensions
{
    public static class CouchApiDbExtensions
    {
        public static CouchApiDb Db(this CouchApiRoot element, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException("databaseName");

            var result = new CouchApiDb(element.RequestClient)
                {
                    PathElement = element.PathElement + databaseName.ToLower() + "/"
                };
            return result;
        }

        public static CouchApiDbCommand AllDocs(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_all_docs/"
            };
            return result;
        }

        public static CouchApiDbCommand BulkDocs(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_bulk_docs/"
            };
            return result;
        }

        public static CouchApiDbCommand Changes(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_changes/"
            };
            return result;
        }

        public static CouchApiDbCommand Compact(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_compact/"
            };
            return result;
        }

        public static CouchApiDbCommand EnsureFullCommit(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_ensure_full_commit/"
            };
            return result;
        }

        public static CouchApiDbCommand MissingRevs(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_missing_revs/"
            };
            return result;
        }

        public static CouchApiDbCommand Purge(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_purge/"
            };
            return result;
        }

        public static CouchApiDbCommand RevsDiff(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_revs_diff/"
            };
            return result;
        }

        public static CouchApiDbCommand RevsLimit(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_revs_limit/"
            };
            return result;
        }

        public static CouchApiDbCommand Security(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_security/"
            };
            return result;
        }

        public static CouchApiDbCommand TempView(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_temp_view/"
            };
            return result;
        }

        public static CouchApiDbCommand ViewCleanup(this CouchApiDb element)
        {
            var result = new CouchApiDbCommand(element.RequestClient) {
                PathElement = element.PathElement + "_view_cleanup/"
            };
            return result;
        }

    }
}