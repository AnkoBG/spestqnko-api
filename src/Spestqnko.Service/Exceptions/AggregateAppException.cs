using System.Net;

namespace Spestqnko.Service.Exceptions
{
    /// <summary>
    /// Represents a collection of application-specific errors that can be handled together
    /// </summary>
    public class AggregateAppException : Exception
    {
        /// <summary>
        /// Gets the collection of error messages
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; }

        /// <summary>
        /// Gets the HTTP status code to return
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateAppException"/> class with a default status code of BadRequest
        /// </summary>
        /// <param name="errors">The collection of error messages</param>
        public AggregateAppException(IEnumerable<string> errors) 
            : this(HttpStatusCode.BadRequest, errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateAppException"/> class with a single error message
        /// </summary>
        /// <param name="statusCode">The HTTP status code to return</param>
        /// <param name="error">The error message</param>
        public AggregateAppException(HttpStatusCode statusCode, string error)
            : this(statusCode, new[] { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateAppException"/> class with a specified status code
        /// </summary>
        /// <param name="statusCode">The HTTP status code to return</param>
        /// <param name="errors">The collection of error messages</param>
        public AggregateAppException(HttpStatusCode statusCode, IEnumerable<string> errors) 
            : base("One or more errors occurred")
        {
            StatusCode = statusCode;
            Errors = errors?.ToList().AsReadOnly() ?? 
                     throw new ArgumentNullException(nameof(errors));
            
            if (!Errors.Any())
            {
                throw new ArgumentException("At least one error message must be provided", nameof(errors));
            }
        }
    }
} 