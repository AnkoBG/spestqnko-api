using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.Wallet
{
    public class CreateWalletInvitationDTO
    {
        [Required(ErrorMessage = "Wallet ID is required")]
        public Guid WalletId { get; set; }
        
        [Range(1, 168, ErrorMessage = "Expiration time must be between 1 and 168 hours (7 days)")]
        public int ExpirationHours { get; set; } = 24; // Default to 24 hours
    }
} 