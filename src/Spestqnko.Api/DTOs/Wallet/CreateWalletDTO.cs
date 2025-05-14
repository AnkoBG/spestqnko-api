using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.Wallet
{
    public class CreateWalletDTO
    {
        [Required(ErrorMessage = "Wallet name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Wallet name must be between 1 and 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Monthly income is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Monthly income must be a positive number")]
        public float MonthlyIncome { get; set; }
    }
} 