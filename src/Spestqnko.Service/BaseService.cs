using Spestqnko.Core;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public abstract class BaseModelService<TEntity> : IService<TEntity> where TEntity : class, IModel
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseModelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return _unitOfWork.GetRepository<TEntity>().GetAllAsync();
        }

        public TEntity GetById(Guid id)
        {
            return _unitOfWork.GetRepository<TEntity>().GetByIdAsync(id);
        }
    }
}