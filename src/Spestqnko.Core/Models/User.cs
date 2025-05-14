using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Core.Models
{
    public class User : IdentityUser<Guid>, IModel
    {
        public new required string UserName { get; set; }
        public required byte[] PWSalt { get; set; }
        public required byte[] PWHash { get; set; }

        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<UserWallet> UserWallets { get; set; } = new List<UserWallet>();
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}