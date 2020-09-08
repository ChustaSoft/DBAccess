﻿using System;

namespace ChustaSoft.Tools.DBAccess
{
    public class EntityFrameworkSearchParametersBuilder<TEntity, TParameters> 
        : SearchParametersBuilderBase<TEntity, EntityFrameworkSearchParameters<TEntity>>, ISearchParametersBuilder<TEntity>, IEntityFrameworkSearchParametersBuilder<TEntity>
        where TEntity : class
        where TParameters : SearchParameters<TEntity>, new()
    {

        protected EntityFrameworkSearchParametersBuilder()
            : base()
        { }


        internal static EntityFrameworkSearchParameters<TEntity> GetParametersFromCriteria(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var builder = new EntityFrameworkSearchParametersBuilder<TEntity, TParameters>();
            searchCriteria.Invoke(builder);
            var searchParams = builder.Build();

            return searchParams;
        }


        public ISearchParametersBuilder<TEntity> WithTracking(bool enabled) 
        {
            _searchParameters.TrackingEnabled = enabled;

            return this;
        }

    }
}
