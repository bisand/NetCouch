namespace Biseth.Net.Settee.Linq
{
    internal class InvalidQueryException : System.Exception
    {
        private readonly string _message;

        public InvalidQueryException(string message)
        {
            _message = message + " ";
        }

        public override string Message
        {
            get { return "The client query is invalid: " + _message; }
        }
    }
}