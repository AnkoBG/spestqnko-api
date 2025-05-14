using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;

namespace Spestqnko.Service
{
    public class WalletService : BaseService<Wallet>, IWalletService
    {
        public WalletService(IRepositoryManager repositoryManager)
            : base(repositoryManager)
        {
        }

        /// <summary>
        /// Creates a new wallet and associates it with the specified user
        /// </summary>
        public async Task<Wallet> CreateWalletAsync(string walletName, Guid userId, float allocatedIncome)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(walletName))
            {
                errors.Add("Wallet name is required");
            }
            else if (walletName.Length < 3)
            {
                errors.Add("Wallet name must be at least 3 characters long");
            }

            if (allocatedIncome < 0)
            {
                errors.Add("Allocated income cannot be negative");
            }

            if (errors.Count > 0)
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, errors);
            }

            // Verify that the user exists
            var user = await _repositoryManager.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new AggregateAppException(HttpStatusCode.NotFound, $"User with ID {userId} not found");
            }

            // Use user's currency if available
            Currency currency = await _repositoryManager.Currencies.GetByIdAsync(user.CurrencyId) 
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"Currency with ID {user.CurrencyId} not found");

            // Create the wallet
            var wallet = new Wallet
            {
                Name = walletName,
                CurrencyId = currency.Id
            };

            // Create the user-wallet association
            var userWallet = new UserWallet
            {
                UserId = userId,
                User = user,
                WalletId = wallet.Id,
                Wallet = wallet,
                AllocatedIncome = allocatedIncome
            };

            // Add the entities
            await _repositoryManager.Wallets.AddAsync(wallet);
            await _repositoryManager.UserWallets.AddAsync(userWallet);
            
            // Save changes
            await _repositoryManager.SaveChangesAsync();

            return wallet;
        }
    }
} 