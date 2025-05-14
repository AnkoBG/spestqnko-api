using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("ExpenseTresholds")]
    public class ExpenseTreshold : IModel
    {
        public Guid Id { get; set; }

        public Guid UserWalletCategoryId { get; set; }

        [ForeignKey("UserWalletCategoryId")]
        public required UserWalletCategory UserWalletCategory { get; set; }

        [Range(0, 100)]
        public int Percent { get; set; }
    }
} 