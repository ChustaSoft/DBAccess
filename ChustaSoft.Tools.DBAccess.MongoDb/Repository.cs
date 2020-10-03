using ChustaSoft.Common.Contracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public class Repository<TEntity, TKey> : RepositoryBase<IMongoContext, TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IKeyable<TKey>
    {
        private readonly IMongoCollection<TEntity> _dbSet;

        public IQueryable<TEntity> Query => throw new NotImplementedException();


        public Repository(IMongoContext context) 
            : base(context)
        {
            _dbSet = context.GetCollection<TEntity>();
        }

        public TEntity GetSingle(TKey id)
        {
            var idFilter = CreateIdFilter(id);

            return _dbSet
                .Find(idFilter)
                .FirstOrDefault();
        }

        public TEntity GetSingle(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = MongoSearchParametersBuilder<TEntity, MongoSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            return _dbSet.Find(searchParams.Filter).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            var searchParams = MongoSearchParametersBuilder<TEntity, MongoSearchParameters<TEntity>>.GetParametersFromCriteria(searchCriteria);

            return _dbSet.Find(searchParams.Filter).ToList();
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
            var idFilter = CreateIdFilter(entity.Id);

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
            var idFilter = CreateIdFilter(id);

            _dbSet.DeleteOne(idFilter);
        }

        public void Delete(TEntity entity)
        {
            var idFilter = CreateIdFilter(entity.Id);

            _dbSet.DeleteOne(idFilter);
        }


        protected override IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        private FilterDefinition<TEntity> CreateIdFilter<T>(T id)
            => Builders<TEntity>.Filter.Eq("_id", id);

    }



    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
        where TEntity : class, IKeyable<Guid>
    {

        public Repository(IMongoContext context)
            : base(context)
        { }

    }

}
