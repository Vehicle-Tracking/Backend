#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Avt.Web.Backend.Data.Spec;
using Microsoft.EntityFrameworkCore;
using IDataContext = Avt.Web.Backend.Data.Spec.IDataContext;

#endregion

namespace Avt.Web.Backend.Data.Base
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity :
    class, IEntity<TKey>

    {
        protected readonly IDataContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository() : this(new DataContext()) { }

        protected Repository(IDataContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.SetEntity<TEntity>();
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.AsQueryable().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey key)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Id.Equals(key));
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity);
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            DbContext.SetEntityEntry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            if (entity != null) Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Attach(entity);
            DbContext.SetEntityEntry(entity).State = EntityState.Modified;
        }
        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeletePermanentlyAsync(params object[] keyValues)
        {
            return await DeletePermanentlyAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);
            
            if (entity == null)
                return false;

            DbContext.SetEntityEntry(entity).State = EntityState.Modified;
            DbSet.Attach(entity);
            return true;
        }
        protected virtual IQueryable<TEntity> Queryable(bool trackable = false)
        {
            return trackable ? DbSet.AsQueryable() : DbSet.AsQueryable().AsNoTracking();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await DbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await DbSet.FindAsync(cancellationToken, keyValues);
        }

        internal IQueryable<TEntity> Select(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, List<Expression<Func<TEntity, object>>> includes = null, int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query.AsNoTracking();
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, List<Expression<Func<TEntity, object>>> includes = null, int? page = null, int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public virtual void InsertOrUpdate(TEntity entity)
        {
            DbContext.SetEntityEntry(entity).State = IsNull(entity.Id) ? EntityState.Added : EntityState.Modified;
            DbSet.Attach(entity);
        }

        public async Task CommitAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        private static bool IsNull(TKey key)
        {
            if (null == key)
                return true;
            if (key is long || key is int || key is short)
                return Convert.ToInt64(key) == 0;
            if (key is float || key is double)
                return Math.Abs(Convert.ToDouble(key)) < .00000001;
            if (key is byte)
                return Convert.ToByte(key) == 0x0;
            if (key is Guid)
                return new Guid(key.ToString()) == Guid.Empty;
            if (key is string)
                return string.Equals(key as string, null, StringComparison.Ordinal) ||
                       string.IsNullOrWhiteSpace(key.ToString());
            return false;
        }
    }
}