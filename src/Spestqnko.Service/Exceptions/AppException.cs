using System.Globalization;
using System.Net;

namespace Spestqnko.Service.Exceptions
{
    public class AppException : Exception
    {
        public AppException() : base() { }
        public AppException(HttpStatusCode statusCode) : base() { StatusCode = statusCode; }

        public AppException(string message) : base(message) { }
        public AppException(HttpStatusCode statusCode, string message) : base(message) { StatusCode = statusCode; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    }
}
