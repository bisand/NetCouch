using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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
        private bool _disposed;

        public CouchDbSession(ICouchApi api)
        {
            _api = api;
            _storedEntities = new HashSet<object>();
        }

        public CouchDbQuery<T> Query<T>()
        {
            var query = new CouchDbQuery<T>(new CouchDbQueryProvider<T>(_api, new CouchDbTranslation()));
            return query;
        }

        public void Store<T>(T entity)
        {
            var proxy = new CouchObjectProxy<T>(entity);
            _storedEntities.Add(proxy);
        }

        public void SaveChanges()
        {
            var responseData =
                _api.Root()
                    .Db(_api.DefaultDatabase)
                    .BulkDocs()
                    .Post<BulkDocsRequest, BulkDocsResponse>(new BulkDocsRequest(_storedEntities));
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

    public class CouchObjectProxy<T> : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private readonly T _entity;

        public CouchObjectProxy(T entity)
        {
            _entity = entity;
            SetProperties(entity);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Select(x => x.Key);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            return _dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        private void SetProperties(object entity)
        {
            var entityType = _entity.GetType();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = property.GetValue(entity, null);
                _dictionary[property.Name] = value;
            }
            _dictionary["doc_type"] = entity.GetType().Name.ToLower();
        }
    }
}