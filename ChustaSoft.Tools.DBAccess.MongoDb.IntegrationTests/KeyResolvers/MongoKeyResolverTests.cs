using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests
{
    public class MongoKeyResolverTests : MongoDbIntegrationTestBase
    {
        private readonly IUnitOfWork<int> unitOfWork;

        public MongoKeyResolverTests() : base()
        {
            var testContext = new MongoContext(configuration, new MongoKeyResolver());
            unitOfWork = new UnitOfWork<MongoContext, int>(testContext);
        }

        private readonly CountryWithMongoId france  = new CountryWithMongoId { WeirdId = 3, Name = "France" };
        private readonly CountryWithMongoId netherlands  = new CountryWithMongoId { WeirdId = 4, Name = "The Netherlands" };

        [Fact]
        public void Given_UnitOfWork_When_Getrepository_Then_CanUpdateEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<CountryWithMongoId>();
            InsertCountries(france, netherlands);

            // Act
            france.Name = "République française";
            netherlands.Name = "Koninkrijk der Nederlanden";
            repository.Update(france);

            // Assert
            var currentFrance = repository.Find(france.WeirdId);
            var currentNeherlands = repository.Find(netherlands.WeirdId);

            Assert.Equal("République française", currentFrance.Name);
            Assert.Equal("The Netherlands", currentNeherlands.Name);
        }


        /// <summary>
        /// Inserts given countries and commits transaction
        /// </summary>
        /// <param name="countries"></param>
        private void InsertCountries(params CountryWithMongoId[] countries)
        {
            var repository = unitOfWork.GetRepository<CountryWithMongoId>();
            repository.Insert(countries);
            unitOfWork.CommitTransaction();
        }
    }
}
