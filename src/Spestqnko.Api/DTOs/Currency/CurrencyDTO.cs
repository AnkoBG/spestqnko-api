using System.ComponentModel.DataAnnotations;

namespace Spestqnko.Api.DTOs.Currency
{
    public class CurrencyDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(3)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string Symbol { get; set; } = string.Empty;
    }
} 