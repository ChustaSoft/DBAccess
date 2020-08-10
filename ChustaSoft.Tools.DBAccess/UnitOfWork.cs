using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.DBAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {

        private Hashtable _repositories = new Hashtable();

        private readonly TContext _context;


        public UnitOfWork(TContext context)
        {
            _context = context;
        }


        public virtual IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyable<TKey>
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(RepositoryBase<TEntity, TKey>);

            if (!_repositories.ContainsKey(entityName))
            {
                if (repositoryType.IsGenericTypeDefinition)
                    _repositories.Add(entityName, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
                else
                    _repositories.Add(entityName, Activator.CreateInstance(repositoryType, _context));
            }

            return (IRepository<TEntity, TKey>)_repositories[entityName];
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

    }
}
