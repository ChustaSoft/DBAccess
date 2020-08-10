#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using ChustaSoft.Common.Contracts;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class AsyncUnitOfWork<TContext> : UnitOfWorkBase<TContext>, IAsyncUnitOfWork
        where TContext : DbContext
    {

        public AsyncUnitOfWork(TContext context)
            : base(context)
        { }


        public virtual IAsyncRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyable<TKey>
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(RepositoryBase<TEntity, TKey>);

            CreateRepositoryInstance<TEntity, TKey>(entityName, repositoryType);

            return (IAsyncRepository<TEntity, TKey>)_repositories[entityName];
        }

        public async Task<bool> CommitTransactionAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
