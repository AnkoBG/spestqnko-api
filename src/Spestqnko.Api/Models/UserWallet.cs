using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spestqnko.Api.Models
{
    [Table("UserWallets")]
    public class UserWallet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }

        public float MonthlyIncome { get; set; }
    }
}
