using System;

namespace ChustaSoft.Tools.DBAccess
{
    public class MongoSearchParametersBuilder<TEntity, TParameters> 
        : SearchParametersBuilderBase<TEntity, MongoSearchParameters<TEntity>>, ISearchParametersBuilder<TEntity>
        where TEntity : class
        where TParameters : SearchParameters<TEntity>, new()
    {

        protected MongoSearchParametersBuilder()
            : base()
        { }


        internal static MongoSearchParameters<TEntity> GetParametersFromCriteria(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var builder = new MongoSearchParametersBuilder<TEntity, TParameters>();
            searchCriteria.Invoke(builder);
            var searchParams = builder.Build();

            return searchParams;
        }

    }
}
