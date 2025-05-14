using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Core.Models
{
    public class User : IdentityUser<Guid>, IModel
    {
        public string UserName { get; set; }
        public byte[] PWSalt { get; set; }
        public byte[] PWHash { get; set; }

        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<UserWallet> UserWallets { get; set; } = new List<UserWallet>();
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}