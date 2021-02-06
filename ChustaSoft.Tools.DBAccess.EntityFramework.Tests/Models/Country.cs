using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess.Examples.Models
{
    public class Country : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
