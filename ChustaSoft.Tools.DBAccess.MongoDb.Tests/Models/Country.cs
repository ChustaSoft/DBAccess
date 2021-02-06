using ChustaSoft.Common.Contracts;
using System;

namespace ChustaSoft.Tools.DBAccess.MongoDb.IntegrationTests.Models
{
    public class Country : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
