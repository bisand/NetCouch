using Newtonsoft.Json;

namespace Biseth.Net.Settee.Serialization
{
    class NewtonSoftSerializer : ISerializer
    {
        private JsonSerializer _serializer;

        public NewtonSoftSerializer()
        {
            _serializer = new JsonSerializer();
        }

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