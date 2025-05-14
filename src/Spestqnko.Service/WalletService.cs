using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;

namespace Spestqnko.Service
{
    public class WalletService : BaseModelService<Wallet>, IWalletService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserWalletRepository _userWalletRepository;

        public WalletService(
            IWalletRepository walletRepository, 
            IUserRepository userRepository,
            IUserWalletRepository userWalletRepository,
            DbContext dbContext)
            : base(walletRepository, dbContext)
        {
            _userRepository = userRepository;
            _userWalletRepository = userWalletRepository;
        }

        /// <summary>
        /// Creates a new wallet and associates it with the specified user
        /// </summary>
        public async Task<Wallet> CreateWalletAsync(string walletName, Guid userId, float monthlyIncome)
        {
            if (string.IsNullOrWhiteSpace(walletName))
            {
                throw new AppException(HttpStatusCode.BadRequest, "Wallet name is required");
            }

            // Verify that the user exists
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, $"User with ID {userId} not found");
            }

            // Create the wallet
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Name = walletName
            };

            // Create the user-wallet association
            var userWallet = new UserWallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = user,
                WalletId = wallet.Id,
                Wallet = wallet,
                MonthlyIncome = monthlyIncome
            };

            // Add the entities to the context
            await _repository.AddAsync(wallet);
            await _userWalletRepository.AddAsync(userWallet);
            
            // Save changes
            await _dbContext.SaveChangesAsync();

            return wallet;
        }
    }
} 