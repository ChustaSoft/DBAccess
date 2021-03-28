using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.DBAccess
{
    public class MongoContext : IMongoContext
    {

        private const string CUSTOM_CONVENTION = "ChustaSoft.DBAccess-Conventions";


        private readonly ICollection<Func<Task>> _commands;
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _mongoClient;
        private IClientSessionHandle _session;

        public IKeyResolver KeyResolver { get; private set; }


        public MongoContext(IDatabaseConfiguration dbConfiguration, IKeyResolver keyResolver = null) : this(keyResolver)
        {
            _mongoClient = new MongoClient(dbConfiguration.ConnectionString);
            _database = _mongoClient.GetDatabase(dbConfiguration.DatabaseName);
        }

        public MongoContext(IMongoClient mongoClient, IMongoDatabase mongoDatabase, IKeyResolver keyResolver = null) : this(keyResolver)
        {
            _mongoClient = mongoClient;
            _database = mongoDatabase;
        }

        private MongoContext(IKeyResolver keyResolver = null)
        {
            _commands = new List<Func<Task>>();
            KeyResolver = keyResolver ?? new DefaultKeyResolver();

            RegisterConventions();
        }

        public async Task<int> SaveChangesAsync()
        {
            using (_session = await _mongoClient.StartSessionAsync())
            {
                TryStartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await _session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return GetCollection<T>($"{typeof(T).Name}");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            while (_session != null && _session.IsInTransaction)
                Thread.Sleep(TimeSpan.FromMilliseconds(100));

            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }


        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register(CUSTOM_CONVENTION, pack, t => true);
        }

        private void TryStartTransaction()
        {
            try
            {
                _session.StartTransaction();
            }
            catch (NotSupportedException exception) when (exception.Message == "Standalone servers do not support transactions.")
            {
                var helpLink = "https://github.com/ChustaSoft/DBAccess/wiki#using-the-mongodb-implementation";
                var message = $"Transactions are not supported on standalone MongoDb servers. For available options, please refer to {helpLink}";

                throw new NotSupportedException(message, exception)
                {
                    HelpLink = helpLink
                };
            }
        }
    }
}
