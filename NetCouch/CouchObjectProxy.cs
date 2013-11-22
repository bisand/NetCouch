using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Biseth.Net.Couch
{
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
            _dictionary["doc__type"] = entity.GetType().Name;

            var entityType = _entity.GetType();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = property.GetValue(entity, null);
                _dictionary[property.Name] = value;
            }
        }
    }
}