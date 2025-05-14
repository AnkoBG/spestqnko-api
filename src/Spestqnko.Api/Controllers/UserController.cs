using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spestqnko.Api.Models.User;
using Spestqnko.Api.Settings;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spestqnko.Api.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly AppSettings _appSettings;

        public UserController(ILogger<UserController> logger, IUserService userService, IOptions<AppSettings> appSettings)
            : base(logger)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAll()
        {
            var users = await _userService.GetAll();

            return Ok(users.Select(u => u.UserName));
        }

        [HttpGet("current")]
        public ActionResult<string> GetCurrent()
        {
            var user = User;

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var userResult = await _userService.Authenticate(model.Username, model.Password);

            // Check if authentication returned a user (this should not happen with the current implementation 
            // as Authenticate will throw AppException for invalid credentials)
            if (userResult == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            // return basic user info and authentication token
            return Ok(new
            {
                Id = userResult.Id,
                Username = userResult.UserName,
                Token = CreateJwtTokenString(userResult)
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> AddUserAsync([FromBody]RegisterModel model)
        {
            var user = await _userService.AddUserAsync(model.Username, model.Password);
            return Ok(user.UserName);
        }

        private string CreateJwtTokenString(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}