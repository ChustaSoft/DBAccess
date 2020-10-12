using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        IQueryable<TEntity> Query { get; }

        Task<TEntity> GetSingleAsync(TKey id);

        Task<TEntity> GetSingleAsync(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria);

        Task<IEnumerable<TEntity>> GetMultipleAsync(Action<ISearchParametersBuilder<TEntity>> searchCriteria);

#if NETCOREAPP3_1
        IAsyncEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria);
#endif
    }



    public interface IAsyncReadonlyRepository<TEntity> : IAsyncReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
