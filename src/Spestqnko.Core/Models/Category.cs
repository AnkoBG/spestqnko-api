using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Categories")]
    public class Category : IModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set;} = string.Empty;

        public float MaxSpendingAmount { get; set; }

        public Guid WalletId { get; set; }

        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }

        public List<Spending> Spendings { get; set; }

        public List<UserWalletCategory> UserWalletCategories { get; set; }
    }
}