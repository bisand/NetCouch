using System.Net;

namespace NetCouch.Http
{
    public class ResponseData<T>
    {
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Data { get; set; }
        public T DataDeserialized { get; set; }

        public dynamic DynamicData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Data))
                    return default(dynamic);

                var data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(Data);
                return data;
            }
        }
    }

    public static class ResponseDataExtensions
    {
        public static ResponseData<T> Root<T>(this ResponseData<T> responseData)
        {
            return responseData;
        }
    }
}