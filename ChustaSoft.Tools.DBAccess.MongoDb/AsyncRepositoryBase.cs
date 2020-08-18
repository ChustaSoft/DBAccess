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

        public Task<TEntity> GetSingleAsync(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetMultipleAsync(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
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
        
    }

}
