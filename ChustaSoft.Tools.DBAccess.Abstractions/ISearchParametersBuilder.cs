using ChustaSoft.Common.Builders;
using System;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public interface ISearchParametersBuilder<TEntity> : ISingleResultSearchParametersBuilder<TEntity>
        where TEntity : class
    {
                
        ISearchParametersBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> order, OrderType orderType = OrderType.Ascending);

        ISearchParametersBuilder<TEntity> Paginate(int batchSize, int skippedBatches);

        new ISearchParametersBuilder<TEntity> FilterBy(Expression<Func<TEntity, bool>> filter);

        new ISearchParametersBuilder<TEntity> IncludeProperties(Func<TEntity, SelectablePropertiesBuilder<TEntity>> includeProperties);

    }



    public interface ISingleResultSearchParametersBuilder<TEntity>
        where TEntity : class
    {

        ISingleResultSearchParametersBuilder<TEntity> FilterBy(Expression<Func<TEntity, bool>> filter);

        ISingleResultSearchParametersBuilder<TEntity> IncludeProperties(Func<TEntity, SelectablePropertiesBuilder<TEntity>> includeProperties);

    }

}
