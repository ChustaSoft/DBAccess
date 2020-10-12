#if NETFRAMEWORK
using System.Data.Entity;
using System.Data.Entity.Migrations;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class AsyncRepository<TEntity, TKey> : RepositoryBase<DbContext, TEntity>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {

        protected DbSet<TEntity> _dbSet;

        public IQueryable<TEntity> Query => GetQueryable();

        public AsyncRepository(DbContext context)
            : base(context)
        {        
            _dbSet = context.Set<TEntity>();
        }


        public async Task<TEntity> GetSingleAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> GetSingleAsync(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = EntityFrameworkSearchParametersBuilder<TEntity, EntityFrameworkSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            var query = GetQueryable()
                .TryIncludeProperties(searchParams.IncludedProperties)
                .TrySetFilter(searchParams.Filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetMultipleAsync(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            return await Task.Run(() =>
            {
                var searchParams = EntityFrameworkSearchParametersBuilder<TEntity, EntityFrameworkSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

                var query = GetQueryable()
                    .TryIncludeProperties(searchParams.IncludedProperties)
                    .TrySetFilter(searchParams.Filter)
                    .TrySetOrder(searchParams.Order, searchParams.OrderType)
                    .TrySetPagination(searchParams.BatchSize, searchParams.SkippedBatches);

                return searchParams.TrackingEnabled ? query : query.AsNoTracking();
            });
        }

#if NETCOREAPP3_1

        public IAsyncEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = EntityFrameworkSearchParametersBuilder<TEntity, EntityFrameworkSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            var query = GetQueryable()
                .TryIncludeProperties(searchParams.IncludedProperties)
                .TrySetFilter(searchParams.Filter)
                .TrySetOrder(searchParams.Order, searchParams.OrderType)
                .TrySetPagination(searchParams.BatchSize, searchParams.SkippedBatches);

            return searchParams.TrackingEnabled ? query.AsAsyncEnumerable() : query.AsNoTracking().AsAsyncEnumerable();
        }

#endif

        public async Task<bool> InsertAsync(TEntity entity)
        {
#if NETCORE
            await _dbSet.AddAsync(entity);

            return true;
#else
            return await Task.Run(() =>
            {
                _dbSet.Add(entity);

                return true;
            });
#endif
        }

        public async Task<bool> InsertAsync(IEnumerable<TEntity> entities)
        {
#if NETCORE
            await _dbSet.AddRangeAsync(entities);

            return true;
#else
            return await Task.Run(() =>
            {
                _dbSet.AddRange(entities);

                return true;
            });
#endif
        }


        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
#if NETCORE
                _dbSet.Update(entity);
#else
                _dbSet.AddOrUpdate(entity);
#endif

                return true;
            });
        }

        public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
        {

            return await Task.Run(() =>
            {
#if NETCORE
                _dbSet.UpdateRange(entities);

#else
                foreach(var entity in entities)
                    _dbSet.AddOrUpdate(entity);

#endif
                return true;
            });
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);

            return true;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                _dbSet.Remove(entity);

                return true;
            });
        }


        protected override IQueryable<TEntity> GetQueryable() => _dbSet;

    }



    public class AsyncRepository<TEntity> : AsyncRepository<TEntity, Guid>, IAsyncRepository<TEntity>
        where TEntity : class
    {

        public AsyncRepository(DbContext context)
            : base(context)
        { }

    }

}
