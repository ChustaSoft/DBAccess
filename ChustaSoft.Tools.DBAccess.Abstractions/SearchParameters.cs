using ChustaSoft.Common.Builders;
using System;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public class SearchParameters<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Filter { get; set; }
        public Expression<Func<TEntity, object>> Order { get; set; }
        public SelectablePropertiesBuilder<TEntity> IncludedProperties { get; set; }
        public int? SkippedBatches { get; set; }
        public int? BatchSize { get; set; }
        

        public SearchParameters()
        {
            Filter = null;
            Order = null;
            IncludedProperties = null;
            SkippedBatches = null;
            BatchSize = null;
        }

    }
}
