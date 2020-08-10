using ChustaSoft.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : class, IKeyable<TKey>;

        bool CommitTransaction();

        Task<bool> CommitTransactionAsync();

    }
}
