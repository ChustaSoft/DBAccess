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
            var repositoryTuple = GetRepositoryTuple<TEntity, RepositoryBase<TEntity, TKey>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IRepository<TEntity, TKey>)_repositories[repositoryTuple.EntityName];
        }

        public virtual IAsyncRepository<TEntity, TKey> GetAsyncRepository<TEntity>()
            where TEntity : class
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, AsyncRepositoryBase<TEntity, TKey>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IAsyncRepository<TEntity, TKey>)_repositories[repositoryTuple.EntityName];
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
}
