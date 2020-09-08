namespace ChustaSoft.Tools.DBAccess
{
    public interface IEntityFrameworkSearchParametersBuilder<TEntity>
        where TEntity : class
    {
        ISearchParametersBuilder<TEntity> WithTracking(bool enabled);
    }
}
