using System;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IRepository<TEntity, TKey> : ISyncRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    { }


    public interface IRepository<TEntity> : IRepository<TEntity, Guid> 
        where TEntity : class 
    { }

}
