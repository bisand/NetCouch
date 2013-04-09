using System;
using Newtonsoft.Json;

namespace Biseth.Net.Settee.Serialization
{
    class NewtonsoftSerializer<TIn, TOut> : ISerializer<TIn, TOut>
    {
        public Func<TIn, string> Serialze { get; set; }
        public Func<string, TOut> Deserialze { get; set; }

        public string Serialize(TIn obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public TOut Deserialize(string text)
        {
            return JsonConvert.DeserializeObject<TOut>(text);
        }
    }
}