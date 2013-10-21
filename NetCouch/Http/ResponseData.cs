﻿using System.Net;

namespace Biseth.Net.Couch.Http
{
    public class ResponseData<T>
    {
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Data { get; set; }
        public T DataDeserialized { get; set; }
    }

    public static class ResponseDataExtensions
    {
        public static ResponseData<T> Root<T>(this ResponseData<T> responseData)
        {
            return responseData;
        }
    }
}