using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("UserWalletCategories")]
    public class UserWalletCategory : IModel
    {
        public Guid Id { get; set; }

        public Guid UserWalletId { get; set; }

        [ForeignKey("UserWalletId")]
        public required UserWallet UserWallet { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }

        public float MaxSpendingAmount { get; set;}

        public List<ExpenseTreshold> ExpenseTresholds { get; set; } = new List<ExpenseTreshold>();
    }
}