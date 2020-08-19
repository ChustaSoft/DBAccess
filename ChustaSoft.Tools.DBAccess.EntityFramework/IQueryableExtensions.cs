#if NETFRAMEWORK
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
    public static partial class IQueryableExtensions
    {

        public static IQueryable<TEntity> TryIncludeProperties<TEntity>(this IQueryable<TEntity> query, SelectablePropertiesBuilder<TEntity> includedProperties)
            where TEntity : class
        {
            if (includedProperties != null)
                foreach (var includedProperty in includedProperties.GetSelection())
                    query = query.Include(includedProperty.Name);

            return query;
        }

    }
}
