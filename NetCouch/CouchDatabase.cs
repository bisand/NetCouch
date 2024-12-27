using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Exceptions;
using NetCouch.Http;
using NetCouch.Models.Couch.Database;

namespace NetCouch
{
    public class CouchDatabase : IDisposable
    {
        private static readonly ConcurrentBag<string> _availableDatabases = [];
        private readonly RequestClient _client;
        private CouchApi _api;
        private bool _disposed;

        public CouchDatabase(string serverUrl, string username = null, string password = null)
        {
            ServerUrl = serverUrl;
            _client = new RequestClient(serverUrl, username, password);
            _api = new CouchApi(_client);
        }

        public string ServerUrl { get; set; }

        public string DefaultDatabase
        {
            get { return _api.DefaultDatabase; }
        }

        public ResponseData<HttpGetRoot> Initialize()
        {
            var queryResult = _api.Root().Get<HttpGetRoot>();
            if (queryResult == null || queryResult.StatusCode != HttpStatusCode.OK)
            {
                // An error occurred...
            }
            return queryResult;
        }

        public void ChangeDatabase(string databaseName)
        {
            _api.DefaultDatabase = databaseName;
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
                                     .Put<dynamic, InternalResult>("");

                if ((dbResponse.Body == null || dbResponse.Body.Ok == false) && dbResponse.StatusCode != HttpStatusCode.Created)
                {
                    throw new CouchDbException($"{dbResponse.StatusDescription}: {dbResponse.Body?.Error} -> {dbResponse.Body?.Reason}");
                }
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