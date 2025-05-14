using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spestqnko.Service.Exceptions;

namespace Spestqnko.Api.Attributes
{
    /// <summary>
    /// Handles AppExceptions at the controller or action level.
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
            if (context.Exception is AppException appException)
            {
                // Log the exception
                _logger.LogError(appException, "Application error occurred: {Message}", appException.Message);

                // Create a result with the status code from the exception
                var result = new ObjectResult(new
                {
                    message = appException.Message,
                    statusCode = (int)appException.StatusCode
                })
                {
                    StatusCode = (int)appException.StatusCode
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