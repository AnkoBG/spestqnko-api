﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spestqnko.Api.Models.User;
using Spestqnko.Api.Settings;
using Spestqnko.Core.Services;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spestqnko.Api.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _userService;

        private readonly AppSettings _appSettings;

        public UserController(ILogger<UserController> logger, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Index()
        {
            var users = await _userService.GetAll();

            return Ok(users.Select(u => u.UserName));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> AddUserAsync([FromBody]RegisterModel model)
        {
            var user = await _userService.AddUserAsync(model.Username, model.Password);
            return Ok(user.UserName);
        }
    }
}