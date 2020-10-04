#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using ChustaSoft.Common.Contracts;
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Threading.Tasks;
using ChustaSoft.Common.Contracts;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext, TKey> : UnitOfWorkBase<TContext, TKey>, IUnitOfWork<TKey>
        where TContext : DbContext
    {

        public UnitOfWork(TContext context)
            : base(context)
        { }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity>()
            where TEntity : class, IKeyable<TKey>
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, Repository<TEntity>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.RepositoryKey, repositoryTuple.RepositoryType);

            return (IRepository<TEntity, TKey>)_repositories[repositoryTuple.RepositoryKey];
        }

        public virtual IAsyncRepository<TEntity, TKey> GetAsyncRepository<TEntity>()
            where TEntity : class, IKeyable<TKey>
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, AsyncRepository<TEntity, TKey>>();
            
            CreateRepositoryInstance<TEntity>(repositoryTuple.RepositoryKey, repositoryTuple.RepositoryType);

            return (IAsyncRepository<TEntity, TKey>)_repositories[repositoryTuple.RepositoryKey];
        }

        public bool CommitTransaction()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> CommitTransactionAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }



    public class UnitOfWork<TContext> : UnitOfWork<TContext, Guid>, IUnitOfWork
        where TContext : DbContext
    {

        public UnitOfWork(TContext context) 
            : base(context)
        { }


        IRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, Repository<TEntity>>();
            
            CreateRepositoryInstance<TEntity>(repositoryTuple.RepositoryKey, repositoryTuple.RepositoryType);

            return (IRepository<TEntity>)_repositories[repositoryTuple.RepositoryKey];
        }

        IAsyncRepository<TEntity> IUnitOfWork.GetAsyncRepository<TEntity>()
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, AsyncRepository<TEntity>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.RepositoryKey, repositoryTuple.RepositoryType);

            return (IAsyncRepository<TEntity>)_repositories[repositoryTuple.RepositoryKey];
        }

    }

}
