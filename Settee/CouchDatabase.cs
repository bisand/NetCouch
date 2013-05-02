using System;
using System.ComponentModel;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.Http;

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
            return session;
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