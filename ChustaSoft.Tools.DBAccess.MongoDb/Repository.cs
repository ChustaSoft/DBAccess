using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class Repository<TEntity, TKey> : RepositoryBase<IMongoContext, TEntity>, IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {

        #region Fields

        private readonly IMongoCollection<TEntity> _dbSet;

        private readonly IKeyResolver _keyResolver;

        #endregion


        #region Constructors

        public Repository(IMongoContext mongoContext) 
            : base(mongoContext)
        {
            _dbSet = mongoContext.GetCollection<TEntity>();
            _keyResolver = mongoContext.KeyResolver;
        }
        
        #endregion


        #region Sync Operations

        public IQueryable<TEntity> Query() => GetQueryable();

        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties)
        {
            //TODO: Manage includingProperties from collection

            return GetQueryable();
        }

        public TEntity Find(TKey id)
        {
            var entityFilter = _dbSet.Find<TEntity>(Builders<TEntity>.Filter.Eq("_id", id));

            return entityFilter.FirstOrDefault();
        }

        public void Insert(TEntity entity)
        {
            _dbSet.InsertOne(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.InsertMany(entities);
        }

        public void Update(TEntity entity)
        {
            var idFilter = CreateFilterOnId(entity);

            _dbSet.ReplaceOne(idFilter, entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void Delete(TKey id)
        {
            var idFilter = CreateFilter(id);

            _dbSet.DeleteOne(idFilter);
        }

        public void Delete(TEntity entity)
        {
            var idFilter = CreateFilterOnId(entity);

            _dbSet.DeleteOne(idFilter);
        }

        #endregion

        #region Async Operations

        public async Task<TEntity> FindAsync(TKey id)
        {
            var entityFilter = await _dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));

            return await entityFilter.FirstOrDefaultAsync();
        }

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

        private FilterDefinition<TEntity> CreateFilterOnId(TEntity entity)
        {
            var id = _keyResolver.GetKey<TEntity, TKey>(entity);
            return CreateFilter(id);
        }

        private FilterDefinition<TEntity> CreateFilter<T>(T id)
            => Builders<TEntity>.Filter.Eq("_id", id);

        
        #endregion

        protected override IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();

    }



    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>, IAsyncRepository<TEntity>
        where TEntity : class
    {

        public Repository(IMongoContext context)
            : base(context)
        { }

    }

}
