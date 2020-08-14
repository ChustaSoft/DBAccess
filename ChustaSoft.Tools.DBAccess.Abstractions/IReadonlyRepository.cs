using ChustaSoft.Common.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        TEntity GetSingle(TKey id);

        TEntity GetSingle
            (
                Expression<Func<TEntity, bool>> filter,
                SelectablePropertiesBuilder<TEntity> includedProperties = null
            );

        IEnumerable<TEntity> GetMultiple
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                SelectablePropertiesBuilder<TEntity> includedProperties = null,
                int? skippedBatches = null,
                int? batchSize = null,
                bool trackingEnabled = false
            );
    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
