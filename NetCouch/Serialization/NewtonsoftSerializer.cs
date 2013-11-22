using System;
using Newtonsoft.Json;

namespace Biseth.Net.Couch.Serialization
{
    internal class NewtonsoftSerializer<TIn, TOut> : ISerializer<TIn, TOut>
    {
        public NewtonsoftSerializer()
        {
            SerializeFunc = Serialize;
            DeserializeFunc = Deserialize;
        }

        public Func<TIn, string> SerializeFunc { get; set; }
        public Func<string, TOut> DeserializeFunc { get; set; }

        public string Serialize(TIn obj)
        {
            var result = JsonConvert.SerializeObject(obj);
            return result;
        }

        public TOut Deserialize(string text)
        {
            var result = JsonConvert.DeserializeObject<TOut>(text);
            return result;
        }
    }
}