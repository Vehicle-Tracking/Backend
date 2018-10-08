using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Avt.Web.Backend.Data.Spec
{
    public interface IRepository<TEntity, in TKey> where TEntity : class, IEntity<TKey>
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey key);
        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
       void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void AddRange(IEnumerable<TEntity> entities);
        void InsertOrUpdate(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);

        Task CommitAsync();
    }
}