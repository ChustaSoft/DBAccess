using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        Task<TEntity> GetSingleAsync(TKey id);

        Task<TEntity> GetSingleAsync(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria);

        Task<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria);

#if NETCOREAPP3_1
        IAsyncEnumerable<TEntity> GetMultipleAsAsync(Action<ISearchParametersBuilder<TEntity>> searchCriteria);
#endif
    }



    public interface IAsyncReadonlyRepository<TEntity> : IAsyncReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
