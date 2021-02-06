#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChustaSoft.Tools.DBAccess
{
    public static partial class IQueryableExtensions
    {

        public static IQueryable<TEntity> TryIncludeProperties<TEntity>(this IQueryable<TEntity> queryable, Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties)
            where TEntity : class
        {
            if (includingProperties != null)
            {
                var builder = includingProperties.Invoke(queryable);
                queryable = queryable.Include(builder);
            }

            return queryable;
        }


        private static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, ISelectablePropertiesBuilder selectablePropertiesBuilder)
            where TEntity : class
        {            
            foreach (var includedProperty in selectablePropertiesBuilder.Format())            
                query = query.Include(includedProperty);

            return query;
        }

        private static IEnumerable<string> Format(this ISelectablePropertiesBuilder selectablePropertiesBuilder) 
        {
            var nodeFormattedElements = new List<string>();

            foreach (var nodeElement in selectablePropertiesBuilder.Build())
                nodeFormattedElements.AddRange(FormatNode(nodeElement));

            return nodeFormattedElements;
        }

        private static IEnumerable<string> FormatNode(SelectablePropertiesNode currentNode, string currentValue = null) 
        {
            var nodeFormattedElements = new List<string>();
            var nodeValueBuilder = string.IsNullOrWhiteSpace(currentValue) ? 
                new StringBuilder(currentNode.Property) 
                : 
                new StringBuilder(currentValue).Append('.').Append(currentNode.Property);


            if (currentNode.Nested.Any())
                foreach (var nestedNodeElement in currentNode.Nested)
                    nodeFormattedElements.AddRange(FormatNode(nestedNodeElement, nodeValueBuilder.ToString()));
            else
                nodeFormattedElements.Add(nodeValueBuilder.ToString());


            return nodeFormattedElements;
        }

    }
}
