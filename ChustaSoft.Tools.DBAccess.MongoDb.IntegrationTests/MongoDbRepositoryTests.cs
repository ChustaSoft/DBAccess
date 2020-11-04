using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models;
using System;
using System.Linq;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests
{
    public class MongoDbRepositoryTests : MongoDbIntegrationTestBase
    {
        private  readonly IUnitOfWork unitOfWork;

        public MongoDbRepositoryTests() : base()
        {
            var testContext = new MongoContext(configuration);
            unitOfWork = new UnitOfWork<MongoContext>(testContext);
        }

        private readonly Country france  = new Country { Id = Guid.NewGuid(), Name = "France" };
        private readonly Country belgium  = new Country { Id = Guid.NewGuid(), Name = "Belgium" };
        private readonly Country netherlands  = new Country { Id = Guid.NewGuid(), Name = "The Netherlands" };
        private readonly Country sweden = new Country { Id = Guid.NewGuid(), Name = "Sweden" };
        private readonly Country switserland = new Country { Id = Guid.NewGuid(), Name = "Switserland" };


        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_RepositoryIsNotNull() 
        {
            var repository = unitOfWork.GetRepository<Country>();

            Assert.NotNull(repository);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetAsyncRepository_Then_RepositoryIsNotNull()
        {
            var repository = unitOfWork.GetAsyncRepository<Country>();

            Assert.NotNull(repository);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetAsyncRepositoryAndSync_Then_BothRepositoriesAreNotNull()
        {
            var syncRepository = unitOfWork.GetRepository<Country>();
            var asyncRepository = unitOfWork.GetAsyncRepository<Country>();

            Assert.NotNull(syncRepository);
            Assert.NotNull(asyncRepository);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanInsertAndRetrieveEntity()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            // Act
            InsertCountries(france);
            var retrievedCountry = syncRepository.GetSingle(france.Id);

            // Assert
            Assert.Equal(france.Id, retrievedCountry.Id);
            Assert.Equal(france.Name, retrievedCountry.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanInsertMultipleEntities()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            // Act
            InsertCountries(france, belgium);

            // Assert
            var franceRetrieved = syncRepository.GetSingle(france.Id);
            var belgiumRetrieved = syncRepository.GetSingle(belgium.Id);

            Assert.Equal(france.Id, franceRetrieved.Id);
            Assert.Equal(france.Name, franceRetrieved.Name);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
            Assert.Equal(belgium.Name, belgiumRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanGetEntityBasedOnFilter()
        {
            // Arrange
            InsertCountries(france, belgium);
            var syncRepository = unitOfWork.GetRepository<Country>();

            // Act
            var belgiumRetrieved = syncRepository.GetSingle(x => x.FilterBy(c => c.Name == "Belgium"));

            // Assert
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
            Assert.Equal(belgium.Name, belgiumRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanGetEntitiesBasedOnFilter()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            InsertCountries(france, belgium, netherlands, sweden, switserland);

            // Act
            var countriesWithS = syncRepository.GetMultiple(x => x.FilterBy(c => c.Name.StartsWith("S")));

            // Assert
            Assert.Equal(2, countriesWithS.Count());
            Assert.Equal(new[] { "Sweden", "Switserland" }, countriesWithS.Select(c => c.Name));
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanUpdateEntity()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();
            InsertCountries(france, netherlands);

            // Act
            france.Name = "République française";
            netherlands.Name = "Koninkrijk der Nederlanden";
            syncRepository.Update(france);

            // Assert
            var franceByOriginalName = syncRepository.GetSingle(x => x.FilterBy(c => c.Name == "France"));
            var franceByNewName = syncRepository.GetSingle(x => x.FilterBy(c => c.Name == "République française"));
            var unmodifiedCountry = syncRepository.GetSingle(netherlands.Id);

            Assert.Null(franceByOriginalName);
            Assert.Equal(france.Id, franceByNewName.Id);
            Assert.Equal("The Netherlands", unmodifiedCountry.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanUpdateMultipleEntity()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            InsertCountries(france, belgium, netherlands);

            // Act
            france.Name = "République française";
            belgium.Name = "Koninkrijk België";
            syncRepository.Update(new Country[] { france, belgium });

            // Assert
            var franceRetrievedAfterChange = syncRepository.GetSingle(france.Id);
            var belgiumRetrievedAfterChange = syncRepository.GetSingle(belgium.Id);
            var netherlandsRetrieved = syncRepository.GetSingle(netherlands.Id);

            Assert.Equal("République française", franceRetrievedAfterChange.Name);
            Assert.Equal("Koninkrijk België", belgiumRetrievedAfterChange.Name);
            Assert.Equal("The Netherlands", netherlandsRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanDeleteEntityById()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            InsertCountries(france, belgium);

            // Act
            syncRepository.Delete(france.Id);

            // Assert
            var franceRetrieved = syncRepository.GetSingle(france.Id);
            var belgiumRetrieved = syncRepository.GetSingle(belgium.Id);

            Assert.Null(franceRetrieved);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanDeleteEntity()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            InsertCountries(france, belgium);

            // Act
            syncRepository.Delete(france);

            // Assert
            var franceRetrieved = syncRepository.GetSingle(france.Id);
            var belgiumRetrieved = syncRepository.GetSingle(belgium.Id);

            Assert.Null(franceRetrieved);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetSyncRepository_Then_CanReturnQueryables()
        {
            // Arrange
            var syncRepository = unitOfWork.GetRepository<Country>();

            InsertCountries(france, belgium, netherlands, sweden, switserland);

            // Act
            var countriesWithS = syncRepository.Query
                .Where(c => c.Name.StartsWith("Sw"))
                .Select(c => c.Name)
                .Take(1);

            // Assert
            Assert.Equal(1, countriesWithS.Count());
            Assert.Equal(new[] { "Sweden" }, countriesWithS);
        }

        /// <summary>
        /// Inserts given countries and commits transaction
        /// </summary>
        /// <param name="countries"></param>
        private void InsertCountries(params Country[] countries)
        {
            var syncRepository = unitOfWork.GetRepository<Country>();
            syncRepository.Insert(countries);
            unitOfWork.CommitTransaction();
        }
    }
}
