#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext, TKey> : IUnitOfWork<TKey>
        where TContext : DbContext
    {

        protected Hashtable _repositories = new Hashtable();

        private readonly TContext _context;


        public UnitOfWork(TContext context)
        {
            _context = context;
        }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity>()
            where TEntity : class
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(RepositoryBase<TEntity, TKey>);

            CreateRepositoryInstance<TEntity>(entityName, repositoryType);

            return (IRepository<TEntity, TKey>)_repositories[entityName];
        }

        public virtual IAsyncRepository<TEntity, TKey> GetAsyncRepository<TEntity>()
            where TEntity : class
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(AsyncRepositoryBase<TEntity>);

            CreateRepositoryInstance<TEntity>(entityName, repositoryType);

            return (IAsyncRepository<TEntity, TKey>)_repositories[entityName];
        }

        public bool CommitTransaction()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> CommitTransactionAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }


        protected void CreateRepositoryInstance<TEntity>(string entityName, Type repositoryType)
            where TEntity : class
        {
            if (!_repositories.ContainsKey(entityName))
            {
                if (repositoryType.IsGenericTypeDefinition)
                    _repositories.Add(entityName, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
                else
                    _repositories.Add(entityName, Activator.CreateInstance(repositoryType, _context));
            }
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
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(RepositoryBase<TEntity>);

            CreateRepositoryInstance<TEntity>(entityName, repositoryType);

            return (IRepository<TEntity>)_repositories[entityName];
        }

        IAsyncRepository<TEntity> IUnitOfWork.GetAsyncRepository<TEntity>()
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(AsyncRepositoryBase<TEntity>);

            CreateRepositoryInstance<TEntity>(entityName, repositoryType);

            return (IAsyncRepository<TEntity>)_repositories[entityName];
        }

    }

}
