﻿using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

        TEntity GetSingle(TKey id);

        //TEntity GetSingle
        //    (
        //        Expression<Func<TEntity, bool>> filter = null,
        //        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //        List<Expression<Func<TEntity, object>>> includedProperties = null
        //    );

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
}
