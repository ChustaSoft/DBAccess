using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests
{
    public class RepositoryTests : IntegrationTestBase
    {

        public RepositoryTests() 
            : base()
        {}


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
    }
}
