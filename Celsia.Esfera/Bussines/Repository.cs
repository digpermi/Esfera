using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bussines
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        private readonly TContext context;

        protected Repository(TContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            TEntity entity = await this.context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            this.context.Set<TEntity>().Remove(entity);
            await this.context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await this.context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetAsyncByte(byte id)
        {
            return await this.context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {

            IQueryable<TEntity> query = this.context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            string[] properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string includeProperty in properties)
            {
                query = query.Include(includeProperty);
            }

            return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
        }

        public async Task<TEntity> EditAsync(TEntity entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return entity;
        }
    }
}
