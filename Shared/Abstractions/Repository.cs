using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public class Repository<TEntity, TDbContext>
        : IRepository<TEntity> where TEntity : Entity, IAggregateRoot, new() where TDbContext : DBContext<TEntity>
    {
        protected virtual TDbContext _context { get; set; }
        public IUnitOfWork<TEntity> UnitOfWork => _context;
        public Repository(TDbContext context)
        {
            _context = context;
        }
        public bool Add(TEntity entity)
        {
            return _context.Insertable(entity).ExecuteCommand() > 0;
        }

        public Task<bool> AddAsync(TEntity entity)
        {
            return Task.FromResult(Add(entity));
        }

        public int Remove(TEntity entity)
        {
            return _context.Deleteable(entity).ExecuteCommand();
        }

        public Task<int> RemoveAsync(TEntity entity)
        {
            return Task.FromResult(Remove(entity));
        }

        public Task<int> RemoveAsync(Expression<Func<TEntity, bool>> express)
        {
            return Task.FromResult(_context.Deleteable(express).ExecuteCommand());
        }

        public int Update(TEntity entity)
        {
            return _context.Storageable(entity).ExecuteCommand();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }
    }
    public abstract class Repository<TEntity, TKey, TDbContext>
       : Repository<TEntity, TDbContext>, IRepository<TEntity, TKey>
       where TEntity : Entity<TKey>, IAggregateRoot, new() where TDbContext : DBContext<TEntity>
    {
        public Repository(TDbContext context) : base(context)
        {
        }

        public int Delete(TKey id)
        {
            //var entity = _context.<TEntity>(id);
            //if (entity == null)
            //{
            //    return false;
            //}
            //base._context.Remove(entity);
            return 0;
        }

        public async Task<int> DeleteAsync(TKey id)
        {
            //var entity = await _context.FindAsync<TEntity>(id, cancellationToken);
            //if (entity == null)
            //{
            //    return false;
            //}
            //base._context.Remove(entity);
            return 0;
        }

        public TEntity Get(TKey id)
        {
            return _context.Queryable<TEntity>().InSingle(id);
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await Task.Run(() => { return default(TEntity); });//_context.FindAsync<TEntity>(id, cancellationToken);
        }
    }
}
