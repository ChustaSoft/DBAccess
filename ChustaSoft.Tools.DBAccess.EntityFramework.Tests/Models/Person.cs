using ChustaSoft.Common.Contracts;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class Person : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
