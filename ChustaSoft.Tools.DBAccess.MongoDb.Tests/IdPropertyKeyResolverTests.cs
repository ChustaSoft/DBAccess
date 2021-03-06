using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
    public class IdPropertyKeyResolverTests
    {

        public IdPropertyKeyResolverTests() { }


        [Fact]
        public void Given_Entity_When_ItHasAnId_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new IdPropertyKeyResolver();
            var entity = new NonKeyableIntIdCountry { Id = 4 , Name = "France" };

            // Act
            var id = resolver.GetKey<NonKeyableIntIdCountry, int>(entity);

            // Assert
            Assert.Equal(4, id);
        }

        [Fact]
        public void Given_Entity_When_IdIsDefault_Then_ItShouldRetrieveDefault()
        {
            // Arrange
            var resolver = new IdPropertyKeyResolver();
            var entity = new NonKeyableIntIdCountry { Name = "France" };

            // Act
            var id = resolver.GetKey<NonKeyableIntIdCountry, int>(entity);

            // Assert
            Assert.Equal(0, id);
        }

        [Fact]
        public void Given_Entity_When_DoesNotHaveId_Then_ItShouldThrow()
        {
            // Arrange
            var resolver = new IdPropertyKeyResolver();
            var entity = new IdLessCountry { Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<IdLessCountry, Guid>(entity)
            );
        }

        [Fact]
        public void Given_Entity_When_HasWrongIdType_Then_ItShouldThrow()
        {
            // Arrange
            var resolver = new IdPropertyKeyResolver();
            var entity = new StringIdCountry { Id = "F", Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<StringIdCountry, Guid>(entity)
            );
        }

    }
}
