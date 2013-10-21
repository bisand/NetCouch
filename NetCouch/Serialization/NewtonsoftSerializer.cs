using System;
using Newtonsoft.Json;

namespace Biseth.Net.Couch.Serialization
{
    internal class NewtonsoftSerializer<TIn, TOut> : ISerializer<TIn, TOut>
    {
        public NewtonsoftSerializer()
        {
            SerializeFunc = obj => JsonConvert.SerializeObject(obj);
            DeserializeFunc = JsonConvert.DeserializeObject<TOut>;
        }

        public Func<TIn, string> SerializeFunc { get; set; }
        public Func<string, TOut> DeserializeFunc { get; set; }

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