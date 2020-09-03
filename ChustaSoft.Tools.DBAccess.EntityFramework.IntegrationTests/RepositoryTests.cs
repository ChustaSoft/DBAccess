using ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.Examples.Models;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests
{
    public class RepositoryTests : IntegrationTestBase
    {

        public RepositoryTests() 
            : base()
        {}


        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_RepositoryIsNotNull() 
        {
            var repository = _unitOfWork.GetRepository<Country>();

            Assert.NotNull(repository);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetAsyncRepository_Then_RepositoryIsNotNull()
        {
            var repository = _unitOfWork.GetAsyncRepository<Country>();

            Assert.NotNull(repository);
        }

        [Fact]
        public void Given_UnitOfWork_When_GetAsyncRepositoryAndSync_Then_BothRepositoriesAreNotNull()
        {
            var syncRepository = _unitOfWork.GetRepository<Country>();
            var asyncRepository = _unitOfWork.GetAsyncRepository<Country>();

            Assert.NotNull(syncRepository);
            Assert.NotNull(asyncRepository);
        }

    }
}
