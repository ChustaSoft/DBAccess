using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
    public class MongoKeyResolverTests : IntegrationTestBase
    {

        private readonly BsonPropertyIdCountry france  = new BsonPropertyIdCountry { Id = 3, Name = "France" };
        private readonly BsonPropertyIdCountry netherlands  = new BsonPropertyIdCountry { Id = 4, Name = "The Netherlands" };

        protected readonly IUnitOfWork<int> unitOfWork;


        public MongoKeyResolverTests()
            : base()
        {
            unitOfWork = GetUoW<int>();
        }


        [Fact]
        public void Given_Entity_When_BsonIdAttributePresent_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new BsonPropertyIdCountry { Id = 3, Name = "France" };

            // Act
            var id = resolver.GetKey<BsonPropertyIdCountry, int>(entity);

            // Assert
            Assert.Equal(3, id);
        }

        [Fact]
        public void Given_Entity_When_BsonIdAttributePresentOnField_Then_ItShouldRetrieveTheKey()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new BsonFieldIdCountry { Id = 3, Name = "France" };

            // Act
            var id = resolver.GetKey<BsonFieldIdCountry, int>(entity);

            // Assert
            Assert.Equal(3, id);
        }

        [Fact]
        public void Given_Entity_When_BsonIdAttributeNotPresent_Then_ItShouldThrow()
        {
            // Arrange
            var resolver = new MongoKeyResolver();
            var entity = new NonKeyableIntIdCountry { Id = 3, Name = "France" };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(
                () => resolver.GetKey<NonKeyableIntIdCountry, int>(entity)
            );

            // Assert
            var message = "Type 'ChustaSoft.Tools.DBAccess.MongoDb.UnitTests.MongoKeyResolverTests+NoBsonIdCountry' does not contain a property marked with the BsonId attribute. Please register an implementation of IKeyResolver that can process this type";
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void Given_UnitOfWork_When_Getrepository_Then_CanUpdateEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<BsonPropertyIdCountry>();
            InsertCountries(france, netherlands);

            // Act
            france.Name = "République française";
            netherlands.Name = "Koninkrijk der Nederlanden";
            repository.Update(france);

            // Assert
            var currentFrance = repository.Find(france.Id);
            var currentNeherlands = repository.Find(netherlands.Id);

            Assert.Equal("République française", currentFrance.Name);
            Assert.Equal("The Netherlands", currentNeherlands.Name);
        }


        /// <summary>
        /// Inserts given countries and commits transaction
        /// </summary>
        /// <param name="countries"></param>
        private void InsertCountries(params BsonPropertyIdCountry[] countries)
        {
            var repository = unitOfWork.GetRepository<BsonPropertyIdCountry>();
            repository.Insert(countries);
            unitOfWork.CommitTransaction();
        }

    }
}
