using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public string UserName { get; set; }
        public byte[] PWSalt { get; set; }
        public byte[] PWHash { get; set; }

        public List<Notification> Notifications { get; set; }
        public List<UserWallet> UserWallets { get; set; }
        public List<Spending> Spendings { get; set; }
    }
}