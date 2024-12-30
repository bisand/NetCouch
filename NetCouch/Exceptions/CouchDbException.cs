using System;

namespace NetCouch.Exceptions;

internal class CouchDbException(string message) : Exception(message)
{
}