﻿#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext, TKey> : UnitOfWorkBase<TContext>, IUnitOfWork<TKey>
        where TContext : DbContext
    {

        public UnitOfWork(TContext context)
            : base(context)
        { }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity>()
            where TEntity : class
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, Repository<TEntity, TKey>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IRepository<TEntity, TKey>)_repositories[repositoryTuple.EntityName];
        }

        public virtual IAsyncRepository<TEntity, TKey> GetAsyncRepository<TEntity>()
            where TEntity : class
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, AsyncRepository<TEntity, TKey>>();
            
            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IAsyncRepository<TEntity, TKey>)_repositories[repositoryTuple.EntityName];
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
            
            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IRepository<TEntity>)_repositories[repositoryTuple.EntityName];
        }

        IAsyncRepository<TEntity> IUnitOfWork.GetAsyncRepository<TEntity>()
        {
            var repositoryTuple = GetRepositoryTuple<TEntity, AsyncRepository<TEntity>>();

            CreateRepositoryInstance<TEntity>(repositoryTuple.EntityName, repositoryTuple.RepositoryType);

            return (IAsyncRepository<TEntity>)_repositories[repositoryTuple];
        }

    }

}
