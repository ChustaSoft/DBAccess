using ChustaSoft.Common.Contracts;
using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.UnitTests
{
    public class DefaultKeyResolverTests
    {
        public DefaultKeyResolverTests()
        {
        }

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
            var entity = new NonKeyableCountry { Id = Guid.NewGuid() , Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<NonKeyableCountry, Guid>(entity)
            );

            // Assert
            var message = "Type 'ChustaSoft.Tools.DBAccess.MongoDb.UnitTests.DefaultKeyResolverTests+NonKeyableCountry' does not implement IKeyable. Please register an implementation of IKeyResolver that can process this type";
            Assert.Equal(message, exception.Message);
        }

        private class KeyableCountry : IKeyable<Guid>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        private class NonKeyableCountry
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }

}
