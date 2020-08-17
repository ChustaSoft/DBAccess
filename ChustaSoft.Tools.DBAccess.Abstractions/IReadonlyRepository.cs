using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public interface IReadonlyRepository<TEntity, TKey>
        where TEntity : class
    {

        TEntity GetSingle(TKey id);

        TEntity GetSingle(Action<ISingleResultSearchParametersBuilder<TEntity>> searchCriteria);

        IEnumerable<TEntity> GetMultiple(Action<ISearchParametersBuilder<TEntity>> searchCriteria);
    }


    public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, Guid>
        where TEntity : class
    { }

}
