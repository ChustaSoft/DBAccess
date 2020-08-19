using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public class Repository<TEntity, TKey> : RepositoryBase<IMongoContext, TEntity>, IRepository<TEntity, TKey>
        where TEntity : class
    {

        public Repository(IMongoContext context) 
            : base(context)
        { }



        public TEntity GetSingle(TKey id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }


        protected override IQueryable<TEntity> GetQueryable()
        {
            throw new NotImplementedException();
        }

    }



    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
        where TEntity : class
    {

        public Repository(IMongoContext context)
            : base(context)
        { }

    }

}
