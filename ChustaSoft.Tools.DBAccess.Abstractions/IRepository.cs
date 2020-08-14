using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IRepository<TEntity, TKey> : IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {
        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);

        void Update(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entities);

    }


    public interface IRepository<TEntity> : IRepository<TEntity, Guid> 
        where TEntity : class 
    { }

}
