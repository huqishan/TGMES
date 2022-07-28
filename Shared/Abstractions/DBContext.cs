using Shared.Abstractions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public class DBContext<TEntity> : SqlSugarClient, IUnitOfWork<TEntity> where TEntity : class, new()
    {
        public DBContext(ConnectionConfig config) : base(config)
        {
            TEntity entity = new TEntity();
            var a = entity.GetType();

            base.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(entity.GetType());//执行完数据库就有这个表了
        }

        public Task<int> SaveChangesAsync(TEntity entity)
        {
            return base.Storageable(entity).ExecuteCommandAsync();
        }
    }
}
