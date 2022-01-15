using Microsoft.AspNetCore.Mvc;
using Spestqnko.Core.Services;
using System.Diagnostics;

namespace Spestqnko.Api.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Index()
        {
            var users = await _userService.GetAll();

            return Ok(users.Select(u => u.UserName));
        }

        [HttpGet("add/{username}/{password}")]
        public async Task<ActionResult<string>> AddUserAsync(string username, string password)
        {
            var user = await _userService.AddUserAsync(username, password);
            return Ok(user.UserName);
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult<string> Error()
        {
            return Ok("");
        }
    }
}