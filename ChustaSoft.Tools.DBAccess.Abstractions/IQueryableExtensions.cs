using System;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public static partial class IQueryableExtensions
    {

        public static IQueryable<TEntity> TrySetPagination<TEntity>(this IQueryable<TEntity> query, int? batchSize, int? skippedBatches)
            where TEntity : class
        {
            if (skippedBatches != null && batchSize != null)
                query = query
                    .Skip((skippedBatches.Value - 1) * batchSize.Value)
                    .Take(batchSize.Value);

            return query;
        }

        public static IQueryable<TEntity> TrySetTakeFrom<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> takeFrom, bool inclusive)
           where TEntity : class
        {
            if (takeFrom != null)
                query = query.SkipWhile(takeFrom);

            if (!inclusive)
                query = query.Skip(1);

            return query;
        }

        public static IQueryable<TEntity> TrySetOrder<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, object>> order, OrderType? orderType)
           where TEntity : class
        {
            if (order != null && orderType != null)
                switch (orderType)
                {
                    case OrderType.Descending:
                        query = query.OrderByDescending(order);
                        break;
                    case OrderType.Ascending:
                    default:
                        query = query.OrderBy(order);
                        break;
                }

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
