using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base
{
    [Collection("non-parallel test collection")]
    public class MongoDbIntegrationTestBase
    {
        private MongoClient client;
        protected readonly MongoDatabaseConfiguration configuration;

        public MongoDbIntegrationTestBase()
        {
            configuration = GetConfiguration();
            InitializeEmptyDatabase();
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
