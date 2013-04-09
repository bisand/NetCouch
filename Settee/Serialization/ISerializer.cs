using System;

namespace Biseth.Net.Settee.Serialization
{
    public interface ISerializer<TIn, TOut>
    {
        Func<TIn, string> Serialze { get; set; }
        Func<string, TOut> Deserialze { get; set; }
        string Serialize(TIn obj);
        TOut Deserialize(string text);
    }
}