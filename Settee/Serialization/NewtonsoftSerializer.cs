using Newtonsoft.Json;

namespace Biseth.Net.Settee.Serialization
{
    class NewtonsoftSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}