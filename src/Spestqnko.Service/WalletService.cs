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
                CurrencyId = currency.Id,
                Currency = currency
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

            var categories = CreateMockCategories(wallet);

            // Add the entities
            await _repositoryManager.Wallets.AddAsync(wallet);
            await _repositoryManager.UserWallets.AddAsync(userWallet);
            await _repositoryManager.Categories.AddRangeAsync(categories);

            // Save changes
            await _repositoryManager.SaveChangesAsync();

            return wallet;
        }

        /// <summary>
        /// Allows a user to join an existing wallet with validation for currency compatibility
        /// </summary>
        public async Task<Wallet> JoinWalletAsync(Guid walletId, Guid userId)
        {
            // Verify that the wallet exists
            var wallet = await _repositoryManager.Wallets.GetByIdAsync(walletId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"Wallet with ID {walletId} not found");

            // Load wallet currency
            var walletCurrency = await _repositoryManager.Currencies.GetByIdAsync(wallet.CurrencyId);
            if (walletCurrency == null)
            {
                throw new AggregateAppException(HttpStatusCode.NotFound, $"Currency with ID {wallet.CurrencyId} not found");
            }
            wallet.Currency = walletCurrency;

            // Verify that the user exists
            var user = await _repositoryManager.Users.GetByIdAsync(userId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"User with ID {userId} not found");

            // Load user currency
            var userCurrency = await _repositoryManager.Currencies.GetByIdAsync(user.CurrencyId);
            if (userCurrency == null)
            {
                throw new AggregateAppException(HttpStatusCode.NotFound, $"Currency with ID {user.CurrencyId} not found");
            }
            user.Currency = userCurrency;

            // Check if the user is already associated with the wallet
            var existingUserWallets = _repositoryManager.UserWallets.Find(
                uw => uw.UserId == userId && uw.WalletId == walletId
            );

            if (existingUserWallets.Any())
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, "User is already associated with this wallet");
            }

            // Validate currency compatibility
            if (wallet.CurrencyId != user.CurrencyId)
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, 
                    $"Currency mismatch. Wallet uses {walletCurrency.Code} currency, but user uses {userCurrency.Code} currency");
            }

            // Create the user-wallet association with zero allocated income
            var userWallet = new UserWallet
            {
                UserId = userId,
                User = user,
                WalletId = walletId,
                Wallet = wallet,
                AllocatedIncome = 0f // Default to zero
            };

            // Add the entity
            await _repositoryManager.UserWallets.AddAsync(userWallet);
            
            // Save changes
            await _repositoryManager.SaveChangesAsync();

            return wallet;
        }

        // create 5 mock categories for the wallet
        private List<Category> CreateMockCategories(Wallet wallet)
        {
            var categories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                var category = new Category
                {
                    Name = $"Category {i}",
                    WalletId = wallet.Id,
                    Wallet = wallet,
                    MaxSpendingAmount = wallet.AllocatedIncome
                };

                categories.Add(category);
            }

            return categories;
        }

        /// <summary>
        /// Creates a new wallet invitation
        /// </summary>
        public async Task<WalletInvitation> CreateInvitationAsync(Guid walletId, Guid createdByUserId, int expirationHours)
        {
            if (expirationHours <= 0)
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, "Expiration time must be positive");
            }

            var wallet = await _repositoryManager.Wallets.GetByIdAsync(walletId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"Wallet with ID {walletId} not found");

            var user = await _repositoryManager.Users.GetByIdAsync(createdByUserId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"User with ID {createdByUserId} not found");

            var existingUserWallets = _repositoryManager.UserWallets.Find(
                uw => uw.UserId == createdByUserId && uw.WalletId == walletId
            );

            if (!existingUserWallets.Any())
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, "Only wallet members can create invitations");
            }

            var invitation = new WalletInvitation
            {
                WalletId = walletId,
                Wallet = wallet,
                CreatedByUserId = createdByUserId,
                CreatedByUser = user,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };

            await _repositoryManager.WalletInvitations.AddAsync(invitation);
            await _repositoryManager.SaveChangesAsync();

            return invitation;
        }

        /// <summary>
        /// Accepts a wallet invitation and adds the user to the wallet
        /// </summary>
        public async Task<Wallet> AcceptInvitationAsync(Guid invitationId, Guid userId)
        {
            var invitation = await _repositoryManager.WalletInvitations.GetByIdAsync(invitationId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, "Invalid invitation code");

            if (!invitation.IsValid)
            {
                if (invitation.IsExpired)
                {
                    throw new AggregateAppException(HttpStatusCode.BadRequest, "Invitation has expired");
                }
                throw new AggregateAppException(HttpStatusCode.BadRequest, "Invitation has already been used");
            }

            var user = await _repositoryManager.Users.GetByIdAsync(userId)
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"User with ID {userId} not found");

            var existingUserWallets = _repositoryManager.UserWallets.Find(
                uw => uw.UserId == userId && uw.WalletId == invitation.WalletId
            );

            if (existingUserWallets.Any())
            {
                throw new AggregateAppException(HttpStatusCode.BadRequest, "User is already associated with this wallet");
            }

            invitation.IsUsed = true;
            invitation.UsedByUserId = userId;
            invitation.UsedByUser = user;
            invitation.UsedAt = DateTime.UtcNow;

            var wallet = await JoinWalletAsync(invitation.WalletId, userId);

            return wallet;
        }

        /// <summary>
        /// Gets all invitations for a wallet, optionally filtering only valid invitations
        /// </summary>
        public Task<IEnumerable<WalletInvitation>> GetInvitationsByWalletIdAsync(Guid walletId, bool validOnly = false)
        {
            var invitations = _repositoryManager.WalletInvitations.Find(i => i.WalletId == walletId);
            if (validOnly)
            {
                invitations = invitations.Where(i => i.IsValid);
            }
            return Task.FromResult(invitations.AsEnumerable());
        }
    }
} 