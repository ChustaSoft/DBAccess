using ChustaSoft.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncRepository<TEntity, TKey> : IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

#if NETCORE

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> InsertAsync(IEnumerable<TEntity> entities);

#endif


    }
}
