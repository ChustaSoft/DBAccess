using System;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        TEntity Find(TKey id);

        IQueryable<TEntity> FromAll();

        IQueryable<TEntity> FromQuery(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties);

    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
