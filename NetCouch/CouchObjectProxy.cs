using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Biseth.Net.Couch
{
    [DataContract]
    public class CouchObjectProxy<T> : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private readonly Type _entityType;
        private T _originalEntity;
        private bool _modified;

        public CouchObjectProxy(T entity)
        {
            _entityType = typeof(T);
            _originalEntity = entity;
            SetProperties(entity);
        }

        public CouchObjectProxy()
        {
            _entityType = typeof (T);
        }

        [DataMember(Name = "_id", EmitDefaultValue = false, Order = 0)]
        public string Id { get; set; }

        [DataMember(Name = "_rev", EmitDefaultValue = false, Order = 1)]
        public string Rev { get; set; }

        [DataMember(Name = "doc__type", EmitDefaultValue = false, Order = 2)]
        public string DocType { get; set; }

        public T Entity
        {
            get
            {
                var entity = GetEntity();
                return entity;
            }
        }

        public T OriginalEntity
        {
            get { return _originalEntity; }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool Modified
        {
            get { return _modified; }
        }

        public void ResetEntity()
        {
            _modified = false;
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
            _modified = true;
            _dictionary[binder.Name] = value;
            return true;
        }

        private void SetProperties(T entity)
        {
            DocType = entity.GetType().Name;

            var properties = _entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var value = property.GetValue(entity, null);

                switch (propertyName.ToLower())
                {
                    case "id":
                        if (value != null)
                            Id = value.ToString();
                        break;
                    case "rev":
                        if (value != null)
                            Rev = value.ToString();
                        break;
                    default:
                        _dictionary[propertyName] = value;
                        break;
                }
            }
            _modified = false;
        }

        private T GetEntity()
        {
            var entity = Activator.CreateInstance(_entityType);
            foreach (var s in new[] {"id", "rev"})
            {
                var propertyInfo = _entityType.GetProperty(s, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    propertyInfo.SetValue(entity, Id, null);
            }

            foreach (var kvp in _dictionary)
            {
                var propertyInfo = _entityType.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        int tmpInt;
                        if(kvp.Value != null && int.TryParse(kvp.Value.ToString(), out tmpInt))
                            propertyInfo.SetValue(entity, tmpInt, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(entity, kvp.Value, null);                        
                    }
            }
            return (T) entity;
        }
    }
}