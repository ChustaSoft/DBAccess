using Microsoft.CSharp.RuntimeBinder;
using System;

namespace ChustaSoft.Tools.DBAccess
{
    public class IdPropertyKeyResolver : IKeyResolver
    {

        public TKey GetKey<TEntity, TKey>(TEntity entity)
        {
            dynamic dynamicEntity = entity;

            try
            {
                object id = dynamicEntity.Id;
                return (TKey)id;
            }
            catch (RuntimeBinderException exception)
            {
                var message = $"Type '{entity.GetType()}' does not have an Id property. Please register an implementation of {nameof(IKeyResolver)} that can process this type";
                throw new InvalidOperationException(message, exception);
            }
            catch (InvalidCastException exception)
            {
                var message = $"Type '{entity.GetType()}' has an Id property, but it is not of the expected type. Please register an implementation of {nameof(IKeyResolver)} that can process this type";
                throw new InvalidOperationException(message, exception);
            }
        }

    }
}
