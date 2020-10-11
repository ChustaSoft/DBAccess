using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MongoDB.Driver;
using System;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Base
{
    [Collection("non-parallel test collection")]
    public class MongoDbIntegrationTestBase
    {
        private MongoClient client;
        protected readonly IUnitOfWork unitOfWork;
        private readonly MongoDatabaseConfiguration configuration;

        public MongoDbIntegrationTestBase()
        {
            configuration = GetConfiguration();
            InitializeEmptyDatabase();

            unitOfWork = CreateUnitOfWork();
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

        private IUnitOfWork CreateUnitOfWork()
        {
            var testContext = new MongoContext(configuration);
            return new UnitOfWork<MongoContext>(testContext);
        }
    }
}
