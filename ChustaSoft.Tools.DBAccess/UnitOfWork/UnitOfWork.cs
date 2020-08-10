#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using ChustaSoft.Common.Contracts;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext> : UnitOfWorkBase<TContext>, IUnitOfWork
        where TContext : DbContext
    {

        public UnitOfWork(TContext context)
            : base(context)
        { }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyable<TKey>
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(RepositoryBase<TEntity, TKey>);

            CreateRepositoryInstance<TEntity, TKey>(entityName, repositoryType);

            return (IRepository<TEntity, TKey>)_repositories[entityName];
        }

        public bool CommitTransaction()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
