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


        public IQueryable<TEntity> Query => GetQueryable();


        public Repository(DbContext context)
            : base(context)
        {   
            _dbSet = context.Set<TEntity>();
        }

        internal Repository(DbSet<TEntity> dbSet) 
        {
            _dbSet = dbSet;
        }


        public TEntity GetSingle(TKey id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetSingle(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = EntityFrameworkSearchParametersBuilder<TEntity, EntityFrameworkSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            var query = GetQueryable()
                .TryIncludeProperties(searchParams.IncludedProperties)
                .TrySetFilter(searchParams.Filter);

            return query.FirstOrDefault();
        }       

        public IEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = EntityFrameworkSearchParametersBuilder<TEntity, EntityFrameworkSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            var query = GetQueryable()
                .TryIncludeProperties(searchParams.IncludedProperties)
                .TrySetFilter(searchParams.Filter)
                .TrySetOrder(searchParams.Order, searchParams.OrderType)
                .TrySetTakeFrom(searchParams.TakeFrom, searchParams.TakeFromInclusive)
                .TrySetPagination(searchParams.BatchSize, searchParams.SkippedBatches);

            return searchParams.TrackingEnabled ? query : query.AsNoTracking();
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
