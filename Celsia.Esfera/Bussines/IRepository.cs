using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bussines
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<TEntity> GetAsync(int id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> EditAsync(TEntity entity);

        Task<TEntity> DeleteAsync(int id);
    }
}