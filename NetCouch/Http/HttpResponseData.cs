﻿using System.Net;

namespace Biseth.Net.Couch.Http
{
    public class HttpResponseData
    {
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Data { get; set; }
    }
}