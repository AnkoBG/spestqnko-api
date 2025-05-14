using System.ComponentModel.DataAnnotations;
using Spestqnko.Api.DTOs.Currency;

namespace Spestqnko.Api.DTOs.Wallet
{
    public class WalletDTO
    {
        public string Name { get; set; } = string.Empty;
        public float AllocatedIncome { get; set; }
        public required CurrencyDTO Currency { get; set; }
    }
} 