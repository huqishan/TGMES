using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IRepository<TEntity> where TEntity :class,IEntity, IAggregateRoot,new()
    {
        IUnitOfWork<TEntity> UnitOfWork { get; }
        bool Add(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        int Remove(TEntity entity);
        Task<int> RemoveAsync(TEntity entity);
        Task<int> RemoveAsync(Expression<Func<TEntity, bool>> express);
    }
    public interface IRepository<TEntity, TKey> : IRepository<TEntity> where TEntity :Entity<TKey>, IAggregateRoot,new ()
    {
        int Delete(TKey id);
        Task<int> DeleteAsync(TKey id);
        TEntity Get(TKey id);
        Task<TEntity> GetAsync(TKey id);
    }
}
