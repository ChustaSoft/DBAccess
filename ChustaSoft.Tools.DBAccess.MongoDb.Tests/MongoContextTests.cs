using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{
    public class MongoContextTests
    {
        private const string transactionsNotSupportedMessage = "Standalone servers do not support transactions.";
        private readonly Mock<IMongoClient> mongoClientMock = new Mock<IMongoClient>();
        private readonly Mock<IMongoDatabase> mongoDatabaseMock = new Mock<IMongoDatabase>();
        private MongoContext context;

        [Fact]
        public async Task Given_StandAloneServer_When_TransactionStarted_Then_ItShouldReferToDocumentation()
        {
            // Arrange
            SetupStandAloneServer();

            // Act
            var exception = await Assert.ThrowsAsync<NotSupportedException>(
                () => context.SaveChangesAsync()
            );

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("https://github.com/ChustaSoft/DBAccess/wiki#using-the-mongodb-implementation", exception.HelpLink);

            var expectedMessage = "Transactions are not supported on standalone MongoDb servers. For available options, please refer to https://github.com/ChustaSoft/DBAccess/wiki#using-the-mongodb-implementation";
            Assert.Equal(expectedMessage, exception.Message);
        }

        private void SetupStandAloneServer()
        {
            var clientSessionHandleMock = new Mock<IClientSessionHandle>();

            clientSessionHandleMock
                .Setup(csh => csh.StartTransaction(default))
                .Throws(new NotSupportedException(transactionsNotSupportedMessage));

            mongoClientMock
                .Setup(c => c.StartSessionAsync(default, default))
                .ReturnsAsync(clientSessionHandleMock.Object);

            context = new MongoContext(mongoClientMock.Object, mongoDatabaseMock.Object);
        }
    }
}
