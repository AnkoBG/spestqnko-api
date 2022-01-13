using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spestqnko.Api.Models
{
    [Table("SpendingNotificationTresholds")]
    public class SpendingNotificationTreshold
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserCategory")]
        public int UserCategoryId { get; set; }

        public int Percent { get; set; }
    }
}
