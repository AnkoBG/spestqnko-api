using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using System.Collections.Generic;

namespace Spestqnko.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryManager(DbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public ICategoryRepository Categories => _serviceProvider.GetRequiredService<ICategoryRepository>();
        public INotificationRepository Notifications => _serviceProvider.GetRequiredService<INotificationRepository>();
        public IRoleRepository Roles => _serviceProvider.GetRequiredService<IRoleRepository>();
        public IExpenseTresholdRepository ExpenseTresholds => _serviceProvider.GetRequiredService<IExpenseTresholdRepository>();
        public IExpenseRepository Expenses => _serviceProvider.GetRequiredService<IExpenseRepository>();
        public IUserRepository Users => _serviceProvider.GetRequiredService<IUserRepository>();
        public IUserWalletCategoryRepository UserWalletCategories => _serviceProvider.GetRequiredService<IUserWalletCategoryRepository>();
        public IUserWalletRepository UserWallets => _serviceProvider.GetRequiredService<IUserWalletRepository>();
        public IWalletRepository Wallets => _serviceProvider.GetRequiredService<IWalletRepository>();

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IModel
        {
            return _serviceProvider.GetService<IRepository<TEntity>>() ?? 
                   throw new InvalidOperationException($"Repository for entity {typeof(TEntity).Name} not registered in DI container");
        }
    }
} 