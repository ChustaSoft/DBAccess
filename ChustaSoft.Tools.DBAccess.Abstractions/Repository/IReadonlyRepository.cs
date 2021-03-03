using System;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        TEntity GetSingle(TKey id);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetMultiple(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties);

    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
