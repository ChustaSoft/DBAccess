#if NETFRAMEWORK
using System.Data.Entity;
using System.Data.Entity.Migrations;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class Repository<TEntity, TKey> : RepositoryBase<DbContext, TEntity>, IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {
        #region Fields

        protected DbSet<TEntity> _dbSet;

        #endregion


        #region Constructors

        public Repository(DbContext context)
            : base(context)
        {
            _dbSet = context.Set<TEntity>();
        }

        internal Repository(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
        }

        #endregion


        #region Public Sync Operations

        public IQueryable<TEntity> FromAll() => GetQueryable();

        public IQueryable<TEntity> FromQuery(Func<IQueryable<TEntity>, ISelectablePropertiesBuilder> includingProperties)
        {
            var queryable = GetQueryable()
                .TryIncludeProperties(includingProperties);

            return queryable;
        }

        public TEntity Find(TKey id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            PerformSingleUpdate(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                PerformSingleUpdate(entity);
        }

        public void Delete(TKey id)
        {
            var entity = _dbSet.Find(id);

            PerformSingleDelete(entity);
        }

        public void Delete(TEntity entity)
        {
            PerformSingleDelete(entity);
        }

        #endregion

        #region Public Async Operations

        public async Task<TEntity> FindAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

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

        #endregion

        
        #region Protected overritten methods

        protected override IQueryable<TEntity> GetQueryable() => _dbSet;

        #endregion


        #region Private methods

        private void PerformSingleUpdate(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void PerformSingleDelete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }

        #endregion

    }



    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>, IAsyncRepository<TEntity>
        where TEntity : class
    {

        public Repository(DbContext context) 
            : base(context)
        { }

        internal Repository(DbSet<TEntity> dbSet) 
            : base(dbSet)
        { }

    }
}
