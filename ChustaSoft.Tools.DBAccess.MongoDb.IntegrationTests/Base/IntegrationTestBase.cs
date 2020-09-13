using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base
{
    [Collection("non-parallel test collection")]
    public class IntegrationTestBase
    {
        private const string integrationTestConnectionString = "mongodb://localhost:27017";
        private const string databaseName = "ChustaSoftIntegrationTestDb";


        private MongoClient client;
        protected readonly IUnitOfWork unitOfWork;

        public IntegrationTestBase()
        {
            InitializeEmptyDatabase();

            unitOfWork = CreateUnitOfWork();
        }

        private void InitializeEmptyDatabase()
        {
            client = new MongoClient(integrationTestConnectionString);
            client.DropDatabase(databaseName);
        }

        private IUnitOfWork CreateUnitOfWork()
        {
            var configuration = new MongoDatabaseConfiguration(integrationTestConnectionString, databaseName);
            var testContext = new MongoContext(configuration);
            return new UnitOfWork<MongoContext>(testContext);
        }
    }
}
