using System;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties);

        TEntity Find(TKey id);

    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
