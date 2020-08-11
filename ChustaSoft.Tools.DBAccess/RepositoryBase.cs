#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChustaSoft.Common.Builders;

namespace ChustaSoft.Tools.DBAccess
{
    public class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;


        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        internal RepositoryBase(DbSet<TEntity> dbSet) 
        {
            _dbSet = dbSet;
        }


        public TEntity GetSingle(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetMultiple
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                SelectablePropertiesBuilder<TEntity> includedProperties = null,
                int? skippedBatches = null,
                int? batchSize = null,
                bool trackingEnabled = false
            )
        {
            var query = _dbSet;

            query
                .TryIncludeProperties(includedProperties)
                .TrySetFilter(filter)
                .TrySetOrder(orderBy)
                .TrySetPagination(skippedBatches, batchSize);

            return trackingEnabled ? query : query.AsNoTracking();
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
}
