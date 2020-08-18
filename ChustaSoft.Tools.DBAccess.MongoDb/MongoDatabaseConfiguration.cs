﻿namespace ChustaSoft.Tools.DBAccess
{
    public class MongoDatabaseConfiguration : IDatabaseConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }


        public MongoDatabaseConfiguration(string connectionString, string databaseName)
        {
            this.ConnectionString = connectionString;
            this.DatabaseName = databaseName;
        }

    }
}