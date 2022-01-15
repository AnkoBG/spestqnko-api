using Spestqnko.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        INotificationRepository Notifications { get; }
        IRoleRepository Roles { get; }
        ISpendingNotificationTresholdRepository SpendingNotificationTresholds { get; }
        ISpendingRepository Spendings { get; }
        IUserRepository Users { get; }
        IUserWalletCategoryRepository UserWalletCategories { get; }
        IUserWalletRepository UserWallets { get; }
        IWalletRepository Wallets { get; }
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;
        Task<int> CommitAsync();
    }
}
