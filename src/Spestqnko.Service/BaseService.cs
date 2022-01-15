using Spestqnko.Core;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public abstract class BaseService<TEntity> : IService<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return _unitOfWork.GetRepository<TEntity>().GetAllAsync();
        }

        public Task<TEntity> GetById(Guid id)
        {
            return _unitOfWork.GetRepository<TEntity>().GetByIdAsync(id).AsTask();
        }
    }
}