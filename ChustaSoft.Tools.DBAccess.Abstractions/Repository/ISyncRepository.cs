using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public interface ISyncRepository<TEntity, TKey> : IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {
        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entity);

    }


    public interface ISyncRepository<TEntity> : ISyncRepository<TEntity, Guid> 
        where TEntity : class 
    { }

}
