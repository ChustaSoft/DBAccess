using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
    public class DefaultKeyResolverTests
    {

        public DefaultKeyResolverTests() { }


        [Fact]
        public void Given_Entity_When_Keyable_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new DefaultKeyResolver();
            var entity = new KeyableCountry { Id = Guid.NewGuid() , Name = "France" };

            // Act
            var id = resolver.GetKey<KeyableCountry, Guid>(entity);

            // Assert
            Assert.Equal(entity.Id, id);
        }

        [Fact]
        public void Given_Entity_When_NotKeyable_Then_ItShouldThrow()
        {
            // Arrange
            var resolver = new DefaultKeyResolver();
            var entity = new NonKeyableGuidIdCountry { Id = Guid.NewGuid() , Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<NonKeyableGuidIdCountry, Guid>(entity)
            );
        }
       
    }
}
