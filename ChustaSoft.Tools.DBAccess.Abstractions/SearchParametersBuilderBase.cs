using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public class SearchParametersBuilderBase<TEntity, TParameters> : ISearchParametersBuilder<TEntity>, IBuilder<TParameters>
        where TEntity : class
        where TParameters : SearchParameters<TEntity>, new()
    {

        protected TParameters _searchParameters;


        public ICollection<ErrorMessage> Errors { get; set; }


        protected SearchParametersBuilderBase()
        {
            _searchParameters = new TParameters();
        }


        public TParameters Build() => _searchParameters;

        ISingleResultSearchParametersBuilder<TEntity> ISingleResultSearchParametersBuilder<TEntity>.FilterBy(Expression<Func<TEntity, bool>> filter)
        {
            PerformFilterBy(filter);

            return this;
        }

        public ISearchParametersBuilder<TEntity> FilterBy(Expression<Func<TEntity, bool>> filter)
        {
            PerformFilterBy(filter);

            return this;
        }

        ISingleResultSearchParametersBuilder<TEntity> ISingleResultSearchParametersBuilder<TEntity>.IncludeProperties(Func<TEntity, SelectablePropertiesBuilder<TEntity>> includeProperties)
        {
            PerformIncludeProperties(includeProperties);

            return this;
        }

        public ISearchParametersBuilder<TEntity> IncludeProperties(Func<TEntity, SelectablePropertiesBuilder<TEntity>> includeProperties)
        {
            PerformIncludeProperties(includeProperties);

            return this;
        }

        public ISearchParametersBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> order, OrderType orderType = OrderType.Ascending)
        {
            _searchParameters.Order = order;
            _searchParameters.OrderType = orderType;

            return this;
        }

        public ISearchParametersBuilder<TEntity> Paginate(int? batchSize, int? skippedBatches)
        {
            _searchParameters.BatchSize = batchSize;
            _searchParameters.SkippedBatches = skippedBatches;

            return this;
        }


        private void PerformIncludeProperties(Func<TEntity, SelectablePropertiesBuilder<TEntity>> includeProperties)
        {
            var builder = includeProperties.Invoke(SelectablePropertiesBuilder<TEntity>.GetProperties());

            _searchParameters.IncludedProperties = builder;
        }

        private void PerformFilterBy(Expression<Func<TEntity, bool>> filter)
        {
            _searchParameters.Filter = filter;
        }

    }
}
