using ChustaSoft.Common.Contracts;
using System;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : class, IKeyable<TKey>;

        bool CommitTransaction();

    }
}
