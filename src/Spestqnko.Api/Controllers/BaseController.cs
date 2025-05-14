using Microsoft.AspNetCore.Mvc;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected new User? User => HttpContext.Items["User"] as User;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
