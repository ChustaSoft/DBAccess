using ChustaSoft.Common.Builders;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class AsyncRepositoryBase<TEntity, TKey> : IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {

        private readonly IMongoContext _context;
        private readonly IMongoCollection<TEntity> _dbSet;


        public AsyncRepositoryBase(IMongoContext mongoContext)
        {
            _context = mongoContext;
            _dbSet = mongoContext.GetCollection<TEntity>();
        }


        public Task<TEntity> GetSingleAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter, SelectablePropertiesBuilder<TEntity> includedProperties = null)
        {
            throw new NotImplementedException();
        }



#if NETCOREAPP3_1

        public IAsyncEnumerable<TEntity> GetMultipleAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            SelectablePropertiesBuilder<TEntity> includedProperties = null,
            int? skippedBatches = null,
            int? batchSize = null,
            bool trackingEnabled = false)
        {
            throw new NotImplementedException();
        }

#endif

#if NETCORE

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

#endif

        

    }

}
