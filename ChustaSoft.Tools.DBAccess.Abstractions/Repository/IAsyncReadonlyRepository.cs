using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        IQueryable<TEntity> Query { get; }

        Task<TEntity> Find(TKey id);

    }



    public interface IAsyncReadonlyRepository<TEntity> : IAsyncReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
