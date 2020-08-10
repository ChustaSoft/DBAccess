using ChustaSoft.Common.Contracts;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IRepository<TEntity, TKey> : IReadonlyRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {
        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);

        void Update(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entities);

    }
}
