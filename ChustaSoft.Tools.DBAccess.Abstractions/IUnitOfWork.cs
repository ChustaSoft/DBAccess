using ChustaSoft.Common.Contracts;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : class, IKeyable<TKey>;

        bool CommitTransaction();

        Task<bool> CommitTransactionAsync();

    }
}
