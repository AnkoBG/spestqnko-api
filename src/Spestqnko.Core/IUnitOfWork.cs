using Spestqnko.Core.Models;
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
        IExpenseNotificationTresholdRepository ExpenseNotificationTresholds { get; }
        IExpenseRepository Expenses { get; }
        IUserRepository Users { get; }
        IUserWalletCategoryRepository UserWalletCategories { get; }
        IUserWalletRepository UserWallets { get; }
        IWalletRepository Wallets { get; }
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IModel;
        Task<int> CommitAsync();
    }
}
