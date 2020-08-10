#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using ChustaSoft.Common.Contracts;
using System;
using System.Collections;


namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWorkBase<TContext>
        where TContext : DbContext
    {

        protected Hashtable _repositories = new Hashtable();

        protected readonly TContext _context;


        public UnitOfWorkBase(TContext context)
        {
            _context = context;
        }


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void CreateRepositoryInstance<TEntity, TKey>(string entityName, Type repositoryType) 
            where TEntity : class, IKeyable<TKey>
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
}
