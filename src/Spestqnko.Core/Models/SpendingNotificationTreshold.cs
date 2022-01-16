using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("SpendingNotificationTresholds")]
    public class SpendingNotificationTreshold : IModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserWalletCategoryId { get; set; }

        [ForeignKey("UserWalletCategoryId")]
        public UserWalletCategory UserWalletCategory { get; set; }

        [Range(0, 100)]
        public int Percent { get; set; }
    }
}