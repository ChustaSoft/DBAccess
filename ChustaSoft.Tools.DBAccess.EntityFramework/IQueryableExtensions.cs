﻿#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using ChustaSoft.Common.Builders;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public static class IQueryableExtensions
    {

        public static IQueryable<TEntity> TrySetPagination<TEntity>(this IQueryable<TEntity> query, int? skippedBatches, int? batchSize)
            where TEntity : class
        {
            if (skippedBatches != null && batchSize != null)
                query = query
                    .Skip((skippedBatches.Value - 1) * batchSize.Value)
                    .Take(batchSize.Value);

            return query;
        }

        public static IQueryable<TEntity> TryIncludeProperties<TEntity>(this IQueryable<TEntity> query, SelectablePropertiesBuilder<TEntity> includedProperties)
            where TEntity : class
        {
            if (includedProperties != null)
                foreach (var includedProperty in includedProperties.GetSelection())
                    query = query.Include(includedProperty.Name);

            return query;
        }

        public static IQueryable<TEntity> TrySetOrder<TEntity>(this IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
            where TEntity : class
        {
            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public static IQueryable<TEntity> TrySetFilter<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            if (filter != null)
                query = query.Where(filter);

            return query;
        }

    }
}