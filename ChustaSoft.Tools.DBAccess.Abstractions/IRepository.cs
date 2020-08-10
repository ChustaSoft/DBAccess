using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess.Abstractions
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IKeyable<TKey>
    {

        TEntity GetSingle(TKey id);

        IEnumerable<TEntity> GetMultiple(
                Expression<Func<TEntity, bool>> filter = null, 
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
                IList<Expression<Func<TEntity, object>>> includeProperties = null
            );


        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);

        void Update(List<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entities);

    }
}
