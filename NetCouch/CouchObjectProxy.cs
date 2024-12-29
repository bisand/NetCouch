using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace NetCouch
{
    public class CouchObjectProxy<T> : DynamicObject
    {
        private readonly ExpandoObject _expando;
        private readonly IDictionary<string, object?> _dictionary;
        private readonly Type _entityType;
        private T _entity = default!;
        private bool _modified;
        private T _originalEntity;

        public CouchObjectProxy(T entity)
        {
            _entityType = typeof(T);
            _originalEntity = entity;
            _expando = new ExpandoObject();
            _dictionary = _expando;
            Id = string.Empty;
            Rev = string.Empty;
            DocType = string.Empty;
        }

        public CouchObjectProxy()
        {
            _entityType = typeof(T);
            _originalEntity = Activator.CreateInstance<T>();
            _expando = new ExpandoObject();
            _dictionary = _expando;
            Id = string.Empty;
            Rev = string.Empty;
            DocType = string.Empty;
        }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_rev")]
        public string Rev { get; set; }

        [JsonPropertyName("doc__type")]
        public string DocType { get; set; }

        public T Entity
        {
            get
            {
                if (_entity == null || _modified)
                    _entity = GetEntity();
                return _entity;
            }
            set { _entity = value; }
        }

        public T OriginalEntity
        {
            get
            {
                if (_originalEntity == null)
                    _originalEntity = GetEntity();
                return _originalEntity;
            }
        }

        public int Count => _dictionary.Count;

        public bool Modified => _modified;

        public void ResetEntity()
        {
            _modified = false;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            _modified = true;
            _dictionary[binder.Name] = value;
            return true;
        }

        private void SetProperties(T entity)
        {
            if (entity == null)
                return;

            DocType = entity.GetType().Name;

            var properties = _entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var value = property.GetValue(entity, null);

                switch (propertyName.ToLower())
                {
                    case "id":
                        if (value is not null)
                            Id = value?.ToString();
                        break;
                    case "rev":
                        if (value is not null)
                            Rev = value?.ToString();
                        break;
                    default:
                        _dictionary[propertyName] = value;
                        break;
                }
            }
            _modified = false;
        }

        private T? GetEntity()
        {
            var entity = Activator.CreateInstance(_entityType);
            var piId = _entityType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var piRev = _entityType.GetProperty("Rev", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (piId != null) piId.SetValue(entity, Id, null);
            if (piRev != null) piRev.SetValue(entity, Rev, null);

            foreach (var kvp in _dictionary)
            {
                var propertyInfo = _entityType.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var value = Convert.ChangeType(kvp.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(entity, value, null);
                }
            }
            return (T)entity;
        }
    }
}