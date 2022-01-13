using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spestqnko.Api.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }

        public string Name { get; set; }

        public float MaxSpendingAmount { get; set; }
    }
}
