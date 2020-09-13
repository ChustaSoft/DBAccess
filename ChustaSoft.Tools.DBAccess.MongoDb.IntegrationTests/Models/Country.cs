using ChustaSoft.Common.Contracts;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models
{
    public class Country : IKeyable<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
