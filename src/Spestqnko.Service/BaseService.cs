using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public abstract class BaseService<TEntity> : IService<TEntity> where TEntity : class, IModel
    {
        protected readonly IRepositoryManager _repositoryManager;

        public BaseService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _repositoryManager.GetRepository<TEntity>().GetAllAsync();

        public async Task<TEntity?> GetByIdAsync(Guid id)
            => await _repositoryManager.GetRepository<TEntity>().GetByIdAsync(id);
    }
}