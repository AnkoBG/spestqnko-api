using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.Models.User
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
