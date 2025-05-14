using Spestqnko.Core.Models;

namespace Spestqnko.Core.Services
{
    public interface IWalletService : IService<Wallet>
    {
        // No additional methods needed - using base IService<Wallet> methods
        
        /// <summary>
        /// Creates a new wallet and associates it with the specified user
        /// </summary>
        /// <param name="walletName">The name of the wallet to create</param>
        /// <param name="userId">The ID of the user who will own the wallet</param>
        /// <param name="allocatedIncome">The allocated income for this user-wallet association</param>
        /// <returns>The newly created wallet</returns>
        Task<Wallet> CreateWalletAsync(string walletName, Guid userId, float allocatedIncome);
    }
}
