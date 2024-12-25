using System;

namespace NetCouch.Exceptions
{
    internal class CouchDbException : Exception
    {
        public CouchDbException(string message)
            : base(message)
        {
        }
    }
}