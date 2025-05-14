using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected new User User => HttpContext.Items["User"] as User ?? 
            throw new AuthenticationException("User is not authenticated or not found in the current context");

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
