using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace ChustaSoft.Tools.DBAccess
{
    public class MongoKeyResolver : IKeyResolver
    {
        public TKey GetKey<TEntity, TKey>(TEntity entity)
        {
            var memberWithBsonIdAttribute = entity
                .GetType()
                .GetMembers()
                .FirstOrDefault(m => Attribute.IsDefined(m, typeof(BsonIdAttribute)));
            
            if (memberWithBsonIdAttribute == null)
                throw NoAttributeFoundMessage(entity);
            else
                return GetValue<TEntity, TKey>(memberWithBsonIdAttribute, entity);
        }

        private TKey GetValue<TEntity, TKey>(MemberInfo memberWithBsonIdAttribute, TEntity entity)
        {
            switch (memberWithBsonIdAttribute)
            {
                case PropertyInfo propertyWithBsonIdAttribute:
                    return (TKey)propertyWithBsonIdAttribute.GetValue(entity);
                case FieldInfo fieldWithBsonIdAttribute:
                    return (TKey)fieldWithBsonIdAttribute.GetValue(entity);
                default:
                    throw NoAttributeFoundMessage(entity);
            }
        }

        private Exception NoAttributeFoundMessage<TEntity>(TEntity entity)
        {
            var message = $"Type '{entity.GetType()}' does not contain a property marked with the BsonId attribute. Please register an implementation of IKeyResolver that can process this type";
            return new InvalidOperationException(message);
        }
    }
}
