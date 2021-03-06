using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Core.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        TEntity GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
    }
}
