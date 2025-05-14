using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spestqnko.Api.DTOs.User;
using Spestqnko.Api.Settings;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spestqnko.Api.Controllers
{
    /// <summary>
    /// API controller for managing users and authentication
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the UserController
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="userService">The user service for user operations</param>
        /// <param name="appSettings">Application settings containing JWT configuration</param>
        public UserController(
            ILogger<UserController> logger, 
            IUserService userService, 
            IOptions<AppSettings> appSettings)
            : base(logger)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Retrieves all registered users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/user
        /// </remarks>
        /// <returns>A collection of usernames</returns>
        /// <response code="200">Returns the list of usernames</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> GetAll()
            => Ok((await _userService.GetAllAsync()).Select(u => u.UserName));

        /// <summary>
        /// Retrieves the current authenticated user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/user/current
        /// </remarks>
        /// <returns>The current user information</returns>
        /// <response code="200">Returns the current user information</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> GetCurrent()
            => Ok(User);

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/user/authenticate
        ///     {
        ///        "username": "johndoe",
        ///        "password": "Password123!"
        ///     }
        /// </remarks>
        /// <param name="dto">The authentication credentials</param>
        /// <returns>User information and JWT token for authenticated requests</returns>
        /// <response code="200">Returns the user information and token</response>
        /// <response code="400">If the credentials are invalid</response>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateDTO dto)
        {
            var userResult = await _userService.Authenticate(dto.Username, dto.Password);

            // Check if authentication returned a user (this should not happen with the current implementation 
            // as Authenticate will throw AggregateAppException for invalid credentials)
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

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/user/register
        ///     {
        ///        "username": "johndoe",
        ///        "password": "Password123!"
        ///     }
        /// 
        /// Password requirements:
        /// - Minimum 8 characters long
        /// - At least one uppercase letter
        /// - At least one lowercase letter
        /// - At least one digit
        /// - At least one special character (non-alphanumeric)
        /// </remarks>
        /// <param name="dto">The registration information</param>
        /// <returns>The username of the newly registered user</returns>
        /// <response code="200">Returns the registered username</response>
        /// <response code="400">If the registration information is invalid or username is taken</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AddUserAsync([FromBody]RegisterDTO dto)
            => Ok((await _userService.AddUserAsync(dto.Username, dto.Password)).UserName);

        /// <summary>
        /// Creates a JWT token for the authenticated user
        /// </summary>
        /// <param name="user">The user to create the token for</param>
        /// <returns>A JWT token string</returns>
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