using System;
using System.Linq;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
#if DEBUG

    public class MongoDbRepositoryTests : IntegrationTestBase
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly KeyableCountry france = new KeyableCountry { Id = Guid.NewGuid(), Name = "France" };
        private readonly KeyableCountry belgium = new KeyableCountry { Id = Guid.NewGuid(), Name = "Belgium" };
        private readonly KeyableCountry netherlands = new KeyableCountry { Id = Guid.NewGuid(), Name = "The Netherlands" };
        private readonly KeyableCountry sweden = new KeyableCountry { Id = Guid.NewGuid(), Name = "Sweden" };
        private readonly KeyableCountry switserland = new KeyableCountry { Id = Guid.NewGuid(), Name = "Switserland" };


        public MongoDbRepositoryTests()
            : base()
        {
            unitOfWork = GetUoW();
        }


        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanInsertAndRetrieveEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            // Act
            InsertCountries(france);
            var retrievedCountry = repository.Find(france.Id);

            // Assert
            Assert.Equal(france.Id, retrievedCountry.Id);
            Assert.Equal(france.Name, retrievedCountry.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanInsertMultipleEntities()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            // Act
            InsertCountries(france, belgium);

            // Assert
            var franceRetrieved = repository.Find(france.Id);
            var belgiumRetrieved = repository.Find(belgium.Id);

            Assert.Equal(france.Id, franceRetrieved.Id);
            Assert.Equal(france.Name, franceRetrieved.Name);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
            Assert.Equal(belgium.Name, belgiumRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanGetEntityBasedOnFilter()
        {
            // Arrange
            InsertCountries(france, belgium);
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            // Act
            var belgiumRetrieved = repository.FromAll()
                .First(c => c.Name == "Belgium");

            // Assert
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
            Assert.Equal(belgium.Name, belgiumRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanGetEntitiesBasedOnFilter()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            InsertCountries(france, belgium, netherlands, sweden, switserland);

            // Act
            var countriesWithS = repository.FromAll()
                .Where(c => c.Name.StartsWith("S"));

            // Assert
            Assert.Equal(2, countriesWithS.Count());
            Assert.Equal(new[] { "Sweden", "Switserland" }, countriesWithS.Select(c => c.Name));
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanUpdateEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();
            InsertCountries(france, netherlands);

            // Act
            france.Name = "République française";
            netherlands.Name = "Koninkrijk der Nederlanden";
            repository.Update(france);

            // Assert
            var franceByOriginalName = repository.FromAll().FirstOrDefault(c => c.Name == "France");
            var franceByNewName = repository.FromAll().First(c => c.Name == "République française");
            var unmodifiedCountry = repository.Find(netherlands.Id);

            Assert.Null(franceByOriginalName);
            Assert.Equal(france.Id, franceByNewName.Id);
            Assert.Equal("The Netherlands", unmodifiedCountry.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanUpdateMultipleEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            InsertCountries(france, belgium, netherlands);

            // Act
            france.Name = "République française";
            belgium.Name = "Koninkrijk België";
            repository.Update(new KeyableCountry[] { france, belgium });

            // Assert
            var franceRetrievedAfterChange = repository.Find(france.Id);
            var belgiumRetrievedAfterChange = repository.Find(belgium.Id);
            var netherlandsRetrieved = repository.Find(netherlands.Id);

            Assert.Equal("République française", franceRetrievedAfterChange.Name);
            Assert.Equal("Koninkrijk België", belgiumRetrievedAfterChange.Name);
            Assert.Equal("The Netherlands", netherlandsRetrieved.Name);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanDeleteEntityById()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            InsertCountries(france, belgium);

            // Act
            repository.Delete(france.Id);

            // Assert
            var franceRetrieved = repository.Find(france.Id);
            var belgiumRetrieved = repository.Find(belgium.Id);

            Assert.Null(franceRetrieved);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanDeleteEntity()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            InsertCountries(france, belgium);

            // Act
            repository.Delete(france);

            // Assert
            var franceRetrieved = repository.Find(france.Id);
            var belgiumRetrieved = repository.Find(belgium.Id);

            Assert.Null(franceRetrieved);
            Assert.Equal(belgium.Id, belgiumRetrieved.Id);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_CanReturnQueryables()
        {
            // Arrange
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            InsertCountries(france, belgium, netherlands, sweden, switserland);

            // Act
            var countriesWithS = repository.FromAll()
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
        private void InsertCountries(params KeyableCountry[] countries)
        {
            var repository = unitOfWork.GetRepository<KeyableCountry>();
            repository.Insert(countries);
            unitOfWork.CommitTransaction();
        }

    }

#endif
}
