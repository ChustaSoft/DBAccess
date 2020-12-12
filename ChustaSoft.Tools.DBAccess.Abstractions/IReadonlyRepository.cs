using System;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        IQueryable<TEntity> Query { get; }


        TEntity Find(TKey id);

    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
