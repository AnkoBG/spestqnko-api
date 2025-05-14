using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.Models.User
{
    public class RegisterModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
