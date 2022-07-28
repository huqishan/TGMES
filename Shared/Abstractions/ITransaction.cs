using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    /// <summary>
    /// 事务管理接口
    /// </summary>
    public interface ITransaction<TDbContextTransaction>
    {
        /// <summary>
        /// 获取当前事务
        /// </summary>
        /// <returns></returns>
        TDbContextTransaction GetCurrentTransaction();

        /// <summary>
        /// 事务是否开启
        /// </summary>
        bool HasActiveTransaction { get; }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task<TDbContextTransaction> BeginTransactionAsync();
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task CommitTransactionAsync(TDbContextTransaction transaction);
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollbackTransaction();
    }
}
