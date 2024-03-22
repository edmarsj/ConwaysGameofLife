using System.Net;

namespace ConwaysGameofLife.Core.Exceptions
{
    public class KnownException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public KnownException(string errorCode, HttpStatusCode statusCode) : base(errorCode)
        {
            StatusCode = statusCode;
        }
    }
}
