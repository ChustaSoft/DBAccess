#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
#endif

using ChustaSoft.Common.Contracts;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace ChustaSoft.Tools.DBAccess
{
    public class AsyncRepositoryBase<TEntity, TKey> : IAsyncRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {

        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;

        public AsyncRepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public async Task<TEntity> GetSingleAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

#if NETCOREAPP3_1

        public IAsyncEnumerable<TEntity> GetMultipleAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IList<Expression<Func<TEntity, object>>> includeProperties = null,
            int? skippedBatches = null,
            int? batchSize = null,
            bool trackingEnabled = false)
        {
            IQueryable<TEntity> query = _dbSet;

            query
                .TryIncludeProperties(includeProperties)
                .TrySetFilter(filter)
                .TrySetOrder(orderBy)
                .TrySetPagination(skippedBatches, batchSize);

            return trackingEnabled ? query.AsAsyncEnumerable() : query.AsNoTracking().AsAsyncEnumerable();
        }

#endif

#if NETCORE

        public async Task<bool> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return true;
        }

        public async Task<bool> InsertAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);

            return true;
        }

#endif

    }

}
