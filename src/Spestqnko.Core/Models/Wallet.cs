using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Wallets")]
    public class Wallet : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set;} = string.Empty;

        public Guid CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public required Currency Currency { get; set; }

        public List<UserWallet> UserWallets { get; set; } = new List<UserWallet>();

        public List<Category> Categories { get; set; } = new List<Category>();

        public float AllocatedIncome => UserWallets != null && UserWallets.Count > 0 
            ? UserWallets.Sum(uw => uw.AllocatedIncome) 
            : 0;
    }
}