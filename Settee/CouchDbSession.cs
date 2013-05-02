using System;
using System.ComponentModel;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Linq;

namespace Biseth.Net.Settee
{
    public class CouchDbSession : IDisposable
    {
        private readonly ICouchApi _api;
        private readonly Component _component = new Component();
        private bool _disposed;

        public CouchDbSession(ICouchApi api)
        {
            _api = api;
        }

        public CouchDbQuery<T> Query<T>()
        {
            var query = new CouchDbQuery<T>(new CouchDbQueryProvider<T>(_api));
            return query;
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
                _component.Dispose();

            _disposed = true;
        }

        ~CouchDbSession()
        {
            Dispose(false);
        }

        #endregion
    }
}