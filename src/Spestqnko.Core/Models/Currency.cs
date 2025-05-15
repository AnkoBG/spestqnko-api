using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Currencies")]
    public class Currency : IModel
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(3)]
        public required string Code { get; set; }

        [Required]
        [StringLength(5)]
        public required string Symbol { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
} 