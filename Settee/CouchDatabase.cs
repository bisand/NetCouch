using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Models.Couch.Database;
using Biseth.Net.Settee.Models.Couch.DesignDoc;

namespace Biseth.Net.Settee
{
    public class CouchDatabase : IDisposable
    {
        private bool _disposed;
        private readonly RequestClient _client;
        private CouchApi _api;

        public CouchDatabase(string serverUrl)
        {
            _client = new RequestClient(serverUrl);
        }

        public CouchDbSession OpenSession(string databaseName)
        {
            _api = new CouchApi(_client, databaseName);
            var session = new CouchDbSession(_api);
            CreateDatabaseIfMissing(databaseName);
            return session;
        }

        private void CreateDatabaseIfMissing(string databaseName)
        {
            var queryResult = _api.Root()
                                  .Db(_api.DefaultDatabase)
                                  .Get<object>();


            // Create the view if it doess not exist.
            if (queryResult != null && queryResult.StatusCode == HttpStatusCode.NotFound)
            {
                // Retrieve the current design doc.
                var dbResponse = _api.Root()
                                     .Db(_api.DefaultDatabase)
                                     .Put<dynamic, JsonSuccessStatement>("");

                if ((dbResponse.DataDeserialized == null || dbResponse.DataDeserialized.Ok == false) && dbResponse.StatusCode != HttpStatusCode.Created)
                    throw new DataException("An error occurred while creating the database!");
            }
        }

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _client.Dispose();
                //_api.Dispose();
            }
            _disposed = true;
        }

        ~CouchDatabase()
        {
            Dispose(false);
        }

        #endregion
    }
}