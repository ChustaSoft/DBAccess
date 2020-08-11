using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Helpers;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess.UnitTests
{
    [TestClass]
    public class RepositoryBaseUnitTests
    {

        private RepositoryBase<Person, int> _repositoryBase;
        

        [TestInitialize]
        public void TestInit()
        {
            var persons = Builder<Person>.CreateListOfSize(50).All().With(x => x.Address = Builder<Address>.CreateListOfSize(2).Build()).Build();
            var queryable = persons.AsQueryable();

            var dbSet = new Mock<DbSet<Person>>();
            dbSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<Person>())).Callback<Person>(persons.Add);

            _repositoryBase = new RepositoryBase<Person, int>(dbSet.Object);
        }



        [TestMethod]
        public void Given_Repository_When_GetMultiple_Then_MultipleDataRetrived()
        {
            var data = _repositoryBase.GetMultiple(x => x.Id == 10, includedProperties: SelectablePropertiesBuilder<Person>.GetProperties().SelectProperty(x => x.Address)).ToList();

            Assert.IsTrue(data.All(x => x.Address.Any()));
        }
    }


    public class Person : IKeyable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public IEnumerable<Address> Address { get; set; }
    }


    public class Address : IKeyable<int>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Line { get; set; }
    }
}
