using System;
using System.Collections;

namespace ChustaSoft.Tools.DBAccess
{
    public class UnitOfWorkBase<TContext> : IDisposable
        where TContext : IDisposable
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

        protected (string EntityName, Type RepositoryType) GetRepositoryTuple<TEntity, TRepository>()
            where TEntity : class
            where TRepository : class
        {
            var entityName = typeof(TEntity).Name;
            var repositoryType = typeof(TRepository);

            return (entityName, repositoryType);
        }

    }
}