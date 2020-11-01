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


        protected void CreateRepositoryInstance<TEntity>(string repoKey, Type repositoryType)
            where TEntity : class
        {
            if (!_repositories.ContainsKey(repoKey))
            {
                if (repositoryType.IsGenericTypeDefinition)
                    _repositories.Add(repoKey, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
                else
                    _repositories.Add(repoKey, Activator.CreateInstance(repositoryType, _context));
            }
        }

        protected (string RepositoryKey, Type RepositoryType) GetRepositoryTuple<TEntity, TRepository>()
            where TEntity : class
            where TRepository : class
        {
            var repositoryType = typeof(TRepository);
            var repoKey = $"{typeof(TEntity).Name}_{repositoryType.Name.Split('`')[0]}";

            return (repoKey, repositoryType);
        }

    }
}