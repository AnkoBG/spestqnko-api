using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.Wallet
{
    public class AcceptWalletInvitationDTO
    {
        [Required(ErrorMessage = "Invitation code is required")]
        public Guid Id { get; set; }
    }
} 