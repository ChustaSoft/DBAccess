using ChustaSoft.Common.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        public void Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetMultiple(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, SelectablePropertiesBuilder<TEntity> includedProperties = null, int? skippedBatches = null, int? batchSize = null, bool trackingEnabled = false)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(TKey id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, SelectablePropertiesBuilder<TEntity> includedProperties = null)
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

        public void Update(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
