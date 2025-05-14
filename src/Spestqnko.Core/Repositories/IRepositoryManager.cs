using Spestqnko.Core.Models;

namespace Spestqnko.Core.Repositories
{
    /// <summary>
    /// Provides a single access point to all repositories
    /// </summary>
    public interface IRepositoryManager
    {
        ICategoryRepository Categories { get; }
        INotificationRepository Notifications { get; }
        IRoleRepository Roles { get; }
        IExpenseTresholdRepository ExpenseTresholds { get; }
        IExpenseRepository Expenses { get; }
        IUserRepository Users { get; }
        IUserWalletCategoryRepository UserWalletCategories { get; }
        IUserWalletRepository UserWallets { get; }
        IWalletRepository Wallets { get; }
        ICurrencyRepository Currencies { get; }
        
        Task SaveChangesAsync();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IModel;
    }
} 