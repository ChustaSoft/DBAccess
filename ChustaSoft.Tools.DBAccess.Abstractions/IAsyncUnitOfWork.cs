using ChustaSoft.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncUnitOfWork : IDisposable
    {
        IAsyncRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : class, IKeyable<TKey>;

        Task<bool> CommitTransactionAsync();

    }
}
