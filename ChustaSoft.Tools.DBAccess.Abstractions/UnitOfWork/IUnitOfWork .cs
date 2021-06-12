using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IUnitOfWork<TKey> : IDisposable
    {
        IRepository<TEntity, TKey> GetRepository<TEntity>() 
            where TEntity : class;

        bool CommitTransaction();

        Task<bool> CommitTransactionAsync();

    }



    public interface IUnitOfWork : IUnitOfWork<Guid> 
    {

        new IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

    }

}
