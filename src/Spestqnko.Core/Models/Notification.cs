using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spestqnko.Core.Models
{
    [Table("Notifications")]
    public class Notification : IModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set;}

        [ForeignKey("UserId")]
        public required User User { get; set; }

        public string Text { get; set;} = string.Empty;

        public bool Sent { get; set; } = false;
    }
}