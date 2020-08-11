using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

        Task<TEntity> GetSingleAsync(TKey id);

        //Task<TEntity> GetSingleAsync
        //    (
        //        Expression<Func<TEntity, bool>> filter = null,
        //        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //        List<Expression<Func<TEntity, object>>> includedProperties = null
        //    );

#if NETCOREAPP3_1
        IAsyncEnumerable<TEntity> GetMultipleAsync
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                SelectablePropertiesBuilder<TEntity> includedProperties = null,
                int? skippedBatches = null,
                int? batchSize = null,
                bool trackingEnabled = false
            );
#endif
    }
}
