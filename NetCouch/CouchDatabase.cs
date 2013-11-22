using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using Biseth.Net.Couch.Db.Api;
using Biseth.Net.Couch.Db.Api.Extensions;
using Biseth.Net.Couch.Exceptions;
using Biseth.Net.Couch.Http;
using Biseth.Net.Couch.Models.Couch.Database;

namespace Biseth.Net.Couch
{
    public class CouchDatabase : IDisposable
    {
        private static readonly ConcurrentBag<string> _availableDatabases = new ConcurrentBag<string>();
        private readonly RequestClient _client;
        private CouchApi _api;
        private bool _disposed;

        public CouchDatabase(string serverUrl)
        {
            _client = new RequestClient(serverUrl);
            _api = new CouchApi(_client);
            var queryResult = _api.Root().Get<HttpGetRoot>();
            if (queryResult == null || queryResult.StatusCode != HttpStatusCode.OK)
            {
                // An error occurred...
            }
        }

        public CouchDbSession OpenSession(string databaseName)
        {
            _api = new CouchApi(_client, databaseName);
            var session = new CouchDbSession(_api);
            string tmpStr;
            if (!_availableDatabases.TryPeek(out tmpStr))
            {
                CreateDatabaseIfMissing(databaseName);
                _availableDatabases.Add(databaseName);
            }
            return session;
        }

        private void CreateDatabaseIfMissing(string databaseName)
        {
            var queryResult = _api.Root()
                                  .Db(_api.DefaultDatabase)
                                  .Get<DatabaseInfo>();


            // Create the view if it doess not exist.
            if (queryResult != null && queryResult.StatusCode == HttpStatusCode.NotFound)
            {
                // Retrieve the current design doc.
                var dbResponse = _api.Root()
                                     .Db(_api.DefaultDatabase)
                                     .Put<dynamic, JsonSuccessStatement>("");

                if ((dbResponse.DataDeserialized == null || dbResponse.DataDeserialized.Ok == false) && dbResponse.StatusCode != HttpStatusCode.Created)
                    throw new CouchDbException("An error occurred while creating the database!");
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