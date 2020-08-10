using ChustaSoft.Common.Contracts;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

        Task<TEntity> GetSingleAsync(TKey id);

#if NETCOREAPP_31
        Task<IAsyncEnumerable<TEntity>> GetMultipleAsync
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                IList<Expression<Func<TEntity, object>>> includedProperties = null,
                int? skippedBatches = null,
                int? batchSize = null,
                bool trackingEnabled = false
            );
#endif
    }
}
