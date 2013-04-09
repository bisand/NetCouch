using System;
using System.IO;
using System.Net;
using System.Text;

namespace Settee.Threading
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
            ResponseData = new StringBuilder();
        }

        public int BufferReadSize
        {
            get { return BUFFER_SIZE; }
        }

        public WebRequest Request { get; set; }
        public WebResponse Response { get; set; }
        public Stream ResponseStream { get; set; }
        public StringBuilder ResponseData { get; set; }
    }
}