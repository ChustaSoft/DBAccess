namespace ChustaSoft.Tools.DBAccess
{
    public interface IKeyResolver
    {
        TKey GetKey<TEntity, TKey>(TEntity entity);
    }
}
