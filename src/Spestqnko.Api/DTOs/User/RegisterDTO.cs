using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.User
{
    public class RegisterDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public required string Password { get; set; }
    }
} 