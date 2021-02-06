using MongoDB.Bson.Serialization.Attributes;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models
{
    public class CountryWithMongoId
    {
        [BsonId]
        public int WeirdId { get; set; }
        public string Name { get; set; }
    }
}
