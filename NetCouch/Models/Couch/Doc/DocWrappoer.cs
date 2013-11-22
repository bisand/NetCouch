using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Biseth.Net.Couch.Models.Couch.Doc
{
    public class DocWrappoer<T> : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public DocWrappoer(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            var type = obj.GetType();
            var docType = type.Name;
            var properties = type.GetProperties();

            _dictionary.Add("doc__type", docType);
            
            foreach (var property in properties)
            {
                _dictionary.Add(property.Name, property.GetValue(obj, null));
            }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
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
    }
}