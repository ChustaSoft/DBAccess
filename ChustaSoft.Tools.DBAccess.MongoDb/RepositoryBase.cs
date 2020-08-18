using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        public void Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(TKey id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria)
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
    }
}
