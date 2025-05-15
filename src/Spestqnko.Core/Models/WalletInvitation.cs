using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("WalletInvitations")]
    public class WalletInvitation : IModel
    {
        public Guid Id { get; set; }
        
        public Guid WalletId { get; set; }
        
        [ForeignKey("WalletId")]
        public required Wallet Wallet { get; set; }
        
        public Guid CreatedByUserId { get; set; }
        
        [ForeignKey("CreatedByUserId")]
        public required User CreatedByUser { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
        
        public Guid? UsedByUserId { get; set; }
        
        [ForeignKey("UsedByUserId")]
        public User? UsedByUser { get; set; }
        
        public DateTime? UsedAt { get; set; }
        
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        
        public bool IsValid => !IsUsed && !IsExpired;
    }
} 