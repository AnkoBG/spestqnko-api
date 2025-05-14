using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.User
{
    public class AuthenticateDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
} 