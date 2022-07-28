using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IUnitOfWork<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 与Entity默认的SaveChangesAsync方法相同，所以不用实现
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(TEntity entity);
    }
}
