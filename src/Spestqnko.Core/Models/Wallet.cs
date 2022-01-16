using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Wallets")]
    public class Wallet : IModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set;} = string.Empty;

        public List<UserWallet> UserWallets { get; set; }

        public List<Category> Categories { get; set; }
    }
}