#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public class Repository<TEntity, TKey> : RepositoryBase<DbContext, TEntity>, IRepository<TEntity, TKey>
        where TEntity : class
    {

        protected DbSet<TEntity> _dbSet;


        public Repository(DbContext context)
            : base(context)
        {   
            _dbSet = context.Set<TEntity>();
        }

        internal Repository(DbSet<TEntity> dbSet) 
        {
            _dbSet = dbSet;
        }


        public IQueryable<TEntity> Query() => GetQueryable();

        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties)
        {
            var queryable = GetQueryable()
                .TryIncludeProperties(includingProperties);

            return queryable;
        }

        public TEntity Find(TKey id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            PerformSingleUpdate(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                PerformSingleUpdate(entity);
        }

        public void Delete(TKey id)
        {
            var entity = _dbSet.Find(id);

            PerformSingleDelete(entity);
        }

        public void Delete(TEntity entity)
        {
            PerformSingleDelete(entity);
        }


        protected override IQueryable<TEntity> GetQueryable() => _dbSet;


        private void PerformSingleUpdate(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void PerformSingleDelete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }

    }



    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
        where TEntity : class
    {

        public Repository(DbContext context) 
            : base(context)
        { }

        internal Repository(DbSet<TEntity> dbSet) 
            : base(dbSet)
        { }

    }
}
