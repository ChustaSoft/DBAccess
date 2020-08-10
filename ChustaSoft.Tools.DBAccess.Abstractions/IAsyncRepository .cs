using ChustaSoft.Common.Contracts;

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
