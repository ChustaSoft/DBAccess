using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
#if DEBUG

    public class UnitOfWorkTests : IntegrationTestBase
    {

        private readonly IUnitOfWork unitOfWork;


        public UnitOfWorkTests()
            : base()
        {
            unitOfWork = GetUoW();
        }


        [Fact]
        public void Given_UnitOfWork_When_GetRepository_Then_RepositoryIsNotNull()
        {
            var repository = unitOfWork.GetRepository<KeyableCountry>();

            Assert.NotNull(repository);
        }

    }

#endif
}
