using Spestqnko.Core;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpestqnkoDbContext _context;

        private CategoryRepository _categories;
        private NotificationRepository _notifications;
        private RoleRepository _roles;
        private SpendingNotificationTresholdRepository _spendingNotificationTresholds;
        private SpendingRepository _spendings;
        private UserRepository _users;
        private UserWalletCategoryRepository _userWalletCategories;
        private UserWalletRepository _userWallets;
        private WalletRepository _wallets;

        public UnitOfWork(SpestqnkoDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public ISpendingNotificationTresholdRepository SpendingNotificationTresholds => _spendingNotificationTresholds ??= new SpendingNotificationTresholdRepository(_context);
        public ISpendingRepository Spendings => _spendings ??= new SpendingRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IUserWalletCategoryRepository UserWalletCategories => _userWalletCategories ??= new UserWalletCategoryRepository(_context);
        public IUserWalletRepository UserWallets => _userWallets ??= new UserWalletRepository(_context);
        public IWalletRepository Wallets => _wallets ??= new WalletRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            switch (typeof(TEntity)) 
            {
                case Type type when type == typeof(Category):
                    return (IRepository<TEntity>)Categories;
                case Type type when type == typeof(Notification):
                    return (IRepository<TEntity>)Notifications;
                case Type type when type == typeof(Role):
                    return (IRepository<TEntity>)Roles;
                case Type type when type == typeof(SpendingNotificationTreshold):
                    return (IRepository<TEntity>)SpendingNotificationTresholds;
                case Type type when type == typeof(Spending):
                    return (IRepository<TEntity>)Spendings;
                case Type type when type == typeof(User):
                    return (IRepository<TEntity>)Users;
                case Type type when type == typeof(UserWalletCategory):
                    return (IRepository<TEntity>)UserWalletCategories;
                case Type type when type == typeof(UserWallet):
                    return (IRepository<TEntity>)UserWallets;
                case Type type when type == typeof(Wallet):
                    return (IRepository<TEntity>)Wallets;
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
