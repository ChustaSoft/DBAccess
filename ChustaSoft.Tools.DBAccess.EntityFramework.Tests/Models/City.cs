using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class City : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }

        public Country Country { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}