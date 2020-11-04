using ChustaSoft.Common.Contracts;
using System;

namespace ChustaSoft.Tools.DBAccess
{
    public class DefaultKeyResolver : IKeyResolver
    {
        public TKey GetKey<TEntity, TKey>(TEntity entity)
        {
            var keyable = entity
                as IKeyable<TKey>
                ?? throw CreateIsNotKeyableException<TEntity, TKey>(entity);

            return keyable.Id;
        }

        private InvalidOperationException CreateIsNotKeyableException<TEntity, TKey>(TEntity entity)
        {
            var message = $"Type '{entity.GetType()}' does not implement {nameof(IKeyable<TKey>)}. Please register an implementation of {nameof(IKeyResolver)} that can process this type";
            return new InvalidOperationException(message);
        }
    }
}
