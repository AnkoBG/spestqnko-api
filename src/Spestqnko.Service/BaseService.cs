using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public abstract class BaseModelService<TEntity> : IService<TEntity> where TEntity : class, IModel
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly DbContext _dbContext;

        public BaseModelService(IRepository<TEntity> repository, DbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public TEntity? GetById(Guid id)
        {
            return _repository.GetById(id);
        }
    }
}