using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class AsyncRepository<TEntity, TKey> : RepositoryBase<IMongoContext, TEntity>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {

        private readonly IMongoCollection<TEntity> _dbSet;


        public AsyncRepository(IMongoContext mongoContext)
            : base(mongoContext)
        {
            _dbSet = mongoContext.GetCollection<TEntity>();
        }


        public async Task<TEntity> GetSingleAsync(TKey id)
        {
            var entityFilter = await _dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));

            return await entityFilter.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetSingleAsync(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = MongoSearchParametersBuilder<TEntity, MongoSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            var entityFilter = await _dbSet.FindAsync(searchParams.Filter);

            return await entityFilter.FirstOrDefaultAsync();            
        }

        public async Task<IEnumerable<TEntity>> GetMultipleAsync(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = MongoSearchParametersBuilder<TEntity, MongoSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            //TODO: Check if using IQueryable performs better or not

            var entityFilter = await _dbSet.FindAsync(searchParams.Filter);
            var asyncData = await entityFilter.ToListAsync();

            var data = asyncData.AsQueryable()
                .TrySetOrder(searchParams.Order, searchParams.OrderType)
                .TrySetPagination(searchParams.BatchSize, searchParams.SkippedBatches);

            return data;
        }

#if NETCOREAPP3_1

        public IAsyncEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
        }

#endif

        public async Task<bool> InsertAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                _context.AddCommand(async () => await _dbSet.InsertOneAsync(entity));

                return true;
            });
        }

        public async Task<bool> InsertAsync(IEnumerable<TEntity> entities)
        {
            return await Task.Run(() =>
            {
                _context.AddCommand(async () => await _dbSet.InsertManyAsync(entities));

                return true;
            });
        }

        public Task<bool> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }


        protected override IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();
    }



    public class AsyncRepository<TEntity> : AsyncRepository<TEntity, Guid>, IAsyncRepository<TEntity>
    where TEntity : class
    {

        public AsyncRepository(IMongoContext context)
            : base(context)
        { }

    }

}
