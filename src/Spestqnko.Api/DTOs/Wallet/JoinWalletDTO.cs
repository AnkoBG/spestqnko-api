using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.Wallet
{
    public class JoinWalletDTO
    {
        [Required(ErrorMessage = "Wallet ID is required")]
        public Guid WalletId { get; set; }
    }
} 