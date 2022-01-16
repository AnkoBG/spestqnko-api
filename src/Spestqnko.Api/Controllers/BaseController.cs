using Microsoft.AspNetCore.Mvc;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger<UserController> _logger;

        protected new User? User => HttpContext.Items["User"] as User;

        public BaseController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
    }
}
