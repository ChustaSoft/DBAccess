namespace ChustaSoft.Tools.DBAccess
{
    public static class EntityFrameworkSearchParametersBuilderExtensions
    {

        public static ISearchParametersBuilder<TEntity> WithTracking<TEntity>(this ISearchParametersBuilder<TEntity> searchParametersBuilder, bool enable)
            where TEntity : class
        {
            ((IEntityFrameworkSearchParametersBuilder<TEntity>)searchParametersBuilder).WithTracking(enable);


            return searchParametersBuilder;
        }

    }
}
