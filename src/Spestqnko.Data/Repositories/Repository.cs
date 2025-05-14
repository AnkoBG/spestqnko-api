using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.SetIncludeAll<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.SetIncludeAll<TEntity>().ToListAsync();
        }

        public TEntity? GetByIdAsync(Guid id)
        {
            return Context.SetIncludeAll<TEntity>().SingleOrDefault(t => t.Id == id);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
    }

    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> SetIncludeAll<TEntity>(this DbContext context) where TEntity : class, IModel
        {
            var query = context.Set<TEntity>().AsQueryable();

            var entityType = context.Model.FindEntityType(typeof(TEntity));
            if (entityType != null)
            {
                foreach (var property in entityType.GetNavigations())
                    query = query.Include(property.Name);
            }

            return query;
        }
    }
}
