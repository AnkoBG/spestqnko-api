using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Categories")]
    public class Category : IModel
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [Range(0, float.MaxValue)]
        public float? MaxSpendingAmount { get; set; }

        public Guid WalletId { get; set; }

        [ForeignKey("WalletId")]
        public required Wallet Wallet { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}