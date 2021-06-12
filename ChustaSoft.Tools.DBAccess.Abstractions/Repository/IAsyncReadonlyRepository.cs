using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        Task<TEntity> FindAsync(TKey id);

    }



    public interface IAsyncReadonlyRepository<TEntity> : IAsyncReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
