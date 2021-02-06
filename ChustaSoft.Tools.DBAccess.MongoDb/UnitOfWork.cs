using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext, TKey> : UnitOfWorkBase<TContext>, IUnitOfWork<TKey>
        where TContext : IMongoContext
    {

        public UnitOfWork(TContext context)
           : base(context)
        { }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity>()
            where TEntity : class
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, Repository<TEntity, TKey>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.RepositoryKey, repositoryTuple.RepositoryType);

            return (IRepository<TEntity, TKey>)_repositories[repositoryTuple.RepositoryKey];
        }

        public bool CommitTransaction()
        {
            var task = Task.Run(() => CommitTransactionAsync());

            return task.Result;
        }

        public async Task<bool> CommitTransactionAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }


    public class UnitOfWork<TContext> : UnitOfWork<TContext, Guid>, IUnitOfWork
        where TContext : IMongoContext
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

    }

}
