namespace Spestqnko.Api.DTOs.Wallet
{
    public class WalletInvitationDTO
    {
        public Guid Id { get; set; }
        
        public Guid WalletId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsValid { get; set; }
    }
} 