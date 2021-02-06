using MongoDB.Bson.Serialization.Attributes;
using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.UnitTests
{
    public class MongoKeyResolverTests
    {
        public MongoKeyResolverTests()
        {
        }

        [Fact]
        public void Given_Entity_When_BsonIdAttributePresent_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new BsonIdCountry { OtherIdProperty = 3, Name = "France" };

            // Act
            var id = resolver.GetKey<BsonIdCountry, int>(entity);

            // Assert
            Assert.Equal(3, id);
        }

        [Fact]
        public void Given_Entity_When_BsonIdAttributePresentOnField_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new BsonIdOnFieldCountry { OtherIdField = 3, Name = "France" };

            // Act
            var id = resolver.GetKey<BsonIdOnFieldCountry, int>(entity);

            // Assert
            Assert.Equal(3, id);
        }

        [Fact]
        public void Given_Entity_When_BsonIdAttributeNotPresent_Then_ItShouldThrow()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new NoBsonIdCountry { OtherIdProperty = 3, Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<NoBsonIdCountry, int>(entity)
            );

            // Assert
            var message = "Type 'ChustaSoft.Tools.DBAccess.MongoDb.UnitTests.MongoKeyResolverTests+NoBsonIdCountry' does not contain a property marked with the BsonId attribute. Please register an implementation of IKeyResolver that can process this type";
            Assert.Equal(message, exception.Message);
        }

        private class BsonIdCountry
        {
            [BsonId]
            public int OtherIdProperty { get; set; }
            public string Name { get; set; }
        }

        private class BsonIdOnFieldCountry
        {
            [BsonId]
            public int OtherIdField;
            public string Name { get; set; }
        }

        private class NoBsonIdCountry
        {
            public int OtherIdProperty { get; set; }
            public string Name { get; set; }
        }
    }

}
