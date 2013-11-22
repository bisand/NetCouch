using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Biseth.Net.Couch.Db.Api;
using Biseth.Net.Couch.Db.Api.Extensions;
using Biseth.Net.Couch.Linq;
using Biseth.Net.Couch.Models.Couch.Doc;

namespace Biseth.Net.Couch
{
    public class CouchDbSession : IDisposable
    {
        private readonly ICouchApi _api;
        private readonly Component _component = new Component();
        private readonly HashSet<object> _storedEntities;
        private readonly HashSet<object> _trackedEntities;
        private bool _disposed;

        public CouchDbSession(ICouchApi api)
        {
            _api = api;
            _storedEntities = new HashSet<object>();
            _trackedEntities = new HashSet<object>();
        }

        public CouchDbQuery<T> Query<T>()
        {
            var query = new CouchDbQuery<T>(new CouchDbQueryProvider<T>(_api, new CouchDbTranslation(), _trackedEntities));
            return query;
        }

        public void Store<T>(T entity)
        {
            var proxy = new CouchObjectProxy<T>(entity);
            _storedEntities.Add(proxy);
        }

        public void SaveChanges()
        {
            // Persist stored data.
            if (_storedEntities.Any())
            {
                var responseData =
                    _api.Root()
                        .Db(_api.DefaultDatabase)
                        .BulkDocs()
                        .Post<BulkDocsRequest, BulkDocsResponse>(new BulkDocsRequest(_storedEntities));
            }
            if (_trackedEntities.Any())
            {
                // Bulk update tracked entities...
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