using System;

namespace NetCouch.Serialization
{
    public interface ISerializer<TIn, TOut>
    {
        Func<TIn, string> SerializeFunc { get; set; }
        Func<string, TOut> DeserializeFunc { get; set; }
        string Serialize(TIn obj);
        TOut Deserialize(string text);
    }
}