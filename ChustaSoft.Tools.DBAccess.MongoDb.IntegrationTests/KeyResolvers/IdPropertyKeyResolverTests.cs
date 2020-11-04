using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests
{
    public class IdPropertyKeyResolverTests : MongoDbIntegrationTestBase
    {
        private readonly IUnitOfWork<string> unitOfWork;

        public IdPropertyKeyResolverTests() : base()
        {
            var testContext = new MongoContext(configuration, new IdPropertyKeyResolver());
            unitOfWork = new UnitOfWork<MongoContext, string>(testContext);
        }

        private readonly CountryWithIdProperty france  = new CountryWithIdProperty { Id = "F", Name = "France" };
        private readonly CountryWithIdProperty netherlands  = new CountryWithIdProperty { Id = "NL", Name = "The Netherlands" };

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanUpdateEntity()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<CountryWithIdProperty>();
            InsertCountries(france, netherlands);

            // Act
            france.Name = "République française";
            netherlands.Name = "Koninkrijk der Nederlanden";
            syncRepository.Update(france);

            // Assert
            var currentFrance = syncRepository.GetSingle(france.Id);
            var currentNeherlands = syncRepository.GetSingle(netherlands.Id);

            Assert.Equal("République française", currentFrance.Name);
            Assert.Equal("The Netherlands", currentNeherlands.Name);
        }

        /// <summary>
        /// Inserts given countries and commits transaction
        /// </summary>
        /// <param name="countries"></param>
        private void InsertCountries(params CountryWithIdProperty[] countries)
        {
            var syncRepository = unitOfWork.GetRepository<CountryWithIdProperty>();
            syncRepository.Insert(countries);
            unitOfWork.CommitTransaction();
        }
    }
}
