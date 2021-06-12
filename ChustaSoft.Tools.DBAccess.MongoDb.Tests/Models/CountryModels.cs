using ChustaSoft.Common.Contracts;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ChustaSoft.Tools.DBAccess.MongoDb.Tests
{

    public class KeyableCountry : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


    public class NonKeyableIntIdCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class NonKeyableGuidIdCountry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


    public class StringIdCountry
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class BsonPropertyIdCountry
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class BsonFieldIdCountry
    {
        [BsonId]
        public int Id;
        public string Name { get; set; }
    }


    public class IdLessCountry
    {
        public string Name { get; set; }
    }

}
