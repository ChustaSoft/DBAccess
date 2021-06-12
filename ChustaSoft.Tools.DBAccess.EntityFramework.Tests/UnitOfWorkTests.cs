using Xunit;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class UnitOfWorkTests : IntegrationTestBase
    {

        public UnitOfWorkTests() 
            : base()
        {}


        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_RepositoryIsNotNull() 
        {
            var repository = _unitOfWork.GetRepository<Country>();

            Assert.NotNull(repository);
        }

    }
}
