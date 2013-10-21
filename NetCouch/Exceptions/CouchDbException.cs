using System;

namespace Biseth.Net.Couch.Exceptions
{
    internal class CouchDbException : Exception
    {
        public CouchDbException(string message)
            : base(message)
        {
        }
    }
}