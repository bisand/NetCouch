using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Linq;
using NetCouch.Models.Couch.Doc;

namespace NetCouch
{
    public class CouchDbSession : IDisposable
    {
        private readonly ICouchApi _api;
        private readonly Component _component = new Component();
        private readonly List<dynamic> _storedDocuments;
        private readonly List<dynamic> _trackedDocuments;
        private bool _disposed;

        public CouchDbSession(ICouchApi api)
        {
            _api = api;
            _storedDocuments = new List<dynamic>();
            _trackedDocuments = new List<dynamic>();
        }

        public CouchDbQuery<T> Query<T>()
        {
            var query = new CouchDbQuery<T>(new CouchDbQueryProvider<T>(_api, new CouchDbTranslation(), _trackedDocuments));
            return query;
        }

        public void Store<T>(T entity)
        {
            _storedDocuments.Add(entity);
        }

        public T Load<T>(string id)
        {
            var responseData =
                _api.Root()
                    .Db(_api.DefaultDatabase)
                    .Doc(id)
                    .Get<T>();
            
            return responseData.Body;
        }

        public void SaveChanges()
        {
            // Persist stored data.
            if (_storedDocuments.Any())
            {
                var responseData =
                    _api.Root()
                        .Db(_api.DefaultDatabase)
                        .BulkDocs()
                        .Post<BulkDocsRequest, BulkDocsResponse>(new BulkDocsRequest(_storedDocuments));

                //UpdateValues(responseData);
            }
            if (_trackedDocuments.Any(x=>x.Modified))
            {
                var modifiedObjects = _trackedDocuments.Where(x => x.Modified);
                var responseData =
                    _api.Root()
                        .Db(_api.DefaultDatabase)
                        .BulkDocs()
                        .Post<BulkDocsRequest, BulkDocsResponse>(new BulkDocsRequest(modifiedObjects));
                
                UpdateValues(responseData, _trackedDocuments);
                foreach (var o in modifiedObjects)
                    o.ResetEntity();
            }
        }

        private void UpdateValues(ResponseData<BulkDocsResponse> responseData, List<dynamic> documents)
        {
            var storedCount = documents.Count;
            var returnedCount = responseData.Body.Count;
            if (storedCount != returnedCount)
                return;

            var i = 0;
            foreach (var obj in documents)
            {
                var data = responseData.Body[i++];
                if (obj.Id != null && obj.Id != data.Id)
                    continue;

                obj.Id = data.Id;
                obj.Rev = data.Rev;
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