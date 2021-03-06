using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Spendings")]
    public class Spending : IModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public DateTime Date { get; set;}

        public float Amount { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}