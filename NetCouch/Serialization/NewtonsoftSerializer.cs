using System;

namespace NetCouch.Serialization
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
            var result = System.Text.Json.JsonSerializer.Serialize(obj);
            return result;
        }

        public TOut Deserialize(string text)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<TOut>(text);
            return result;
        }
    }
}