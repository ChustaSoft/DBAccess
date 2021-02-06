namespace ChustaSoft.Tools.DBAccess
{
    public class MongoDatabaseConfiguration : IDatabaseConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }


        public MongoDatabaseConfiguration() { }

        public MongoDatabaseConfiguration(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }

    }
}
