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

        private CategoryRepository? _categories;
        private NotificationRepository? _notifications;
        private RoleRepository? _roles;
        private ExpenseNotificationTresholdRepository? _expenseNotificationTresholds;
        private ExpenseRepository? _expenses;
        private UserRepository? _users;
        private UserWalletCategoryRepository? _userWalletCategories;
        private UserWalletRepository? _userWallets;
        private WalletRepository? _wallets;

        public UnitOfWork(SpestqnkoDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public IExpenseNotificationTresholdRepository ExpenseNotificationTresholds => _expenseNotificationTresholds ??= new ExpenseNotificationTresholdRepository(_context);
        public IExpenseRepository Expenses => _expenses ??= new ExpenseRepository(_context);
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

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IModel
        {
            return
                (IRepository<TEntity>)typeof(UnitOfWork).GetProperties()
                .SingleOrDefault(p => typeof(IRepository<TEntity>).IsAssignableFrom(p.PropertyType))
                ?.GetValue(this);
        }
    }
}
