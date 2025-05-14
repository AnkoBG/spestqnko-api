using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spestqnko.Service.Exceptions;

namespace Spestqnko.Api.Attributes
{
    /// <summary>
    /// Handles AggregateAppExceptions at the controller or action level.
    /// Can be applied to individual controllers or actions as an alternative to global middleware.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<AppExceptionFilterAttribute> _logger;

        public AppExceptionFilterAttribute(ILogger<AppExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is AggregateAppException aggregateException)
            {
                // Log the exception
                _logger.LogError(aggregateException, "Application error occurred: {Errors}", 
                    string.Join("; ", aggregateException.Errors));

                // Create a result with the status code from the exception and the collection of errors
                var result = new ObjectResult(new
                {
                    errors = aggregateException.Errors,
                    statusCode = (int)aggregateException.StatusCode
                })
                {
                    StatusCode = (int)aggregateException.StatusCode
                };

                // Set the result
                context.Result = result;
                
                // Mark as handled
                context.ExceptionHandled = true;
            }
            else
            {
                // Let the global middleware handle other types of exceptions
                base.OnException(context);
            }
        }
    }
} 