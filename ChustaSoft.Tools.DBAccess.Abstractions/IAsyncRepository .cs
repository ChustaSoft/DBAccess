using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncRepository<TEntity, TKey> : IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> InsertAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> UpdateAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteAsync(TKey id);

        Task<bool> DeleteAsync(TEntity entity);

    }



    public interface IAsyncRepository<TEntity> : IAsyncRepository<TEntity, Guid>
        where TEntity : class
    { }

}
