using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("UserWallets")]
    public class UserWallet : IModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid WalletId { get; set; }

        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }

        public float MonthlyIncome { get; set;}

        public List<UserWalletCategory> UserWalletCategories { get; set; } = new List<UserWalletCategory>();
    }
}