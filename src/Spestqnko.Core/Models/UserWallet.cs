using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("UserWallets")]
    public class UserWallet : IModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        public Guid WalletId { get; set; }

        [ForeignKey("WalletId")]
        public required Wallet Wallet { get; set; }

        [Range(0, float.MaxValue)]
        public float AllocatedIncome { get; set; }
    }
}