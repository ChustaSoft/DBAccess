using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{

    [Collection("non-parallel test collection")]
    public class IntegrationTestBase
    {

        private MongoClient client;

        protected readonly MongoDatabaseConfiguration configuration;


        public IntegrationTestBase()
        {
            configuration = GetConfiguration();
            InitializeEmptyDatabase();
        }


        protected IUnitOfWork GetUoW() 
        {
            var testContext = new MongoContext(configuration);

            return new UnitOfWork<MongoContext>(testContext);
        }

        protected IUnitOfWork<TKey> GetUoW<TKey>()
        {
            var testContext = new MongoContext(configuration);

            return new UnitOfWork<MongoContext, TKey>(testContext);
        }


        private MongoDatabaseConfiguration GetConfiguration()
        {
            var mongoConfigurationSection = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("MongoDatabaseConfiguration");

            var mongoDatabaseConfiguration = new MongoDatabaseConfiguration();

            ConfigurationBinder.Bind(mongoConfigurationSection, mongoDatabaseConfiguration);

            return mongoDatabaseConfiguration;
        }

        private void InitializeEmptyDatabase()
        {
            client = new MongoClient(configuration.ConnectionString);
            client.DropDatabase(configuration.DatabaseName);
        }

    }
}
