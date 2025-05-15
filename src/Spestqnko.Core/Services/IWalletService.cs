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

        /// <summary>
        /// Allows a user to join an existing wallet with validation for currency compatibility
        /// </summary>
        /// <param name="walletId">The ID of the wallet to join</param>
        /// <param name="userId">The ID of the user who will join the wallet</param>
        /// <returns>The joined wallet with the new user-wallet association</returns>
        Task<Wallet> JoinWalletAsync(Guid walletId, Guid userId);

        /// <summary>
        /// Creates a new wallet invitation
        /// </summary>
        /// <param name="walletId">The ID of the wallet for which to create an invitation</param>
        /// <param name="createdByUserId">The ID of the user creating the invitation</param>
        /// <param name="expirationHours">The number of hours the invitation should be valid</param>
        /// <returns>The created wallet invitation</returns>
        Task<WalletInvitation> CreateInvitationAsync(Guid walletId, Guid createdByUserId, int expirationHours);

        /// <summary>
        /// Accepts a wallet invitation and adds the user to the wallet
        /// </summary>
        /// <param name="invitationId">The invitation id to accept</param>
        /// <param name="userId">The ID of the user accepting the invitation</param>
        /// <returns>The wallet that the user has joined</returns>
        Task<Wallet> AcceptInvitationAsync(Guid invitationId, Guid userId);

        /// <summary>
        /// Gets all invitations for a wallet, optionally filtering only valid invitations
        /// </summary>
        /// <param name="walletId">The wallet ID</param>
        /// <param name="validOnly">If true, only valid invitations are returned</param>
        /// <returns>List of WalletInvitation</returns>
        Task<IEnumerable<WalletInvitation>> GetInvitationsByWalletIdAsync(Guid walletId, bool validOnly = false);
    }
}
