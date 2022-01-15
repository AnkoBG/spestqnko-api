using Microsoft.AspNetCore.Identity;

namespace Spestqnko.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public List<Notification> Notifications { get; set; }
        public List<UserWallet> UserWallets { get; set; }
        public List<Spending> Spendings { get; set; }
    }
}