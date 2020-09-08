using System;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public abstract class RepositoryBase<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {

        protected TContext _context;


        protected RepositoryBase() { }

        protected RepositoryBase(TContext context)
        {
            _context = context;
        }


        protected abstract IQueryable<TEntity> GetQueryable();

    }
}
