using ChustaSoft.Common.Contracts;
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess.Abstractions.UnitTests
{
    public static class SelectablePropertiesBuilderTestHelper
    {

        public static IQueryable<Employee> GetMockedData()
            => Builder<Employee>.CreateListOfSize(50).All()
                    .With(x => x.Address = Builder<Address>.CreateListOfSize(2).Build())
                .Build()
                .AsQueryable();

    }


    public class Employee : IKeyable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Employee Supervisor { get; set; }
        public Company Company { get; set; }

        public IEnumerable<Address> Address { get; set; }
    }


    public class Address : IKeyable<int>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Line { get; set; }
    }

    public class Company : IKeyable<int> 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }

}
