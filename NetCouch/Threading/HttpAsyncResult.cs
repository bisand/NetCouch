using System;
using System.IO;
using System.Net;
using System.Text;

namespace Biseth.Net.Settee.Threading
{
    public class HttpAsyncResult : BasicAsyncResult
    {
        private const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;

        internal HttpAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            Request = null;
            Response = null;
            BufferRead = new byte[BUFFER_SIZE];
            RequestData = new StringBuilder();
            ResponseData = new StringBuilder();
        }

        public int BufferReadSize
        {
            get { return BUFFER_SIZE; }
        }

        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public Stream RequestStream { get; set; }
        public Stream ResponseStream { get; set; }
        public StringBuilder RequestData { get; set; }
        public StringBuilder ResponseData { get; set; }
    }
}