using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Helpers;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess.UnitTests
{
    [TestClass]
    public class RepositoryBaseUnitTests
    {

        private Repository<Person, int> _repositoryBase;
        

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

            _repositoryBase = new Repository<Person, int>(dbSet.Object);
        }


        [TestMethod]
        public void Given_SearchParametersWithFilter_When_GetSingle_Then_SingleObjectRetrivedMatchingCriteria()
        {
            var filteredId = 5;
            var data = _repositoryBase.GetSingle(x => x.FilterBy(y => y.Id == filteredId));

            Assert.IsNotNull(data);
            Assert.AreEqual(filteredId, data.Id);
        }

        [TestMethod]
        public void Given_SearchParametersWithFilterAndIncludedProperties_When_GetSingle_Then_SingleObjectRetrivedMatchingCriteria()
        {
            var filteredId = 5;
            var data = _repositoryBase.GetSingle(x => x
                    .FilterBy(y => y.Id == filteredId)
                    .IncludeProperties(y => y.SelectProperty(z => z.Address))
                );

            Assert.IsNotNull(data);
            Assert.AreEqual(filteredId, data.Id);
            Assert.IsTrue(data.Address.Any());
        }

        [TestMethod]
        public void Given_SearchParametersWithFilterAndIncludedProperties_When_GetMultiple_Then_MultipleDataRetrivedMatchingCriteria()
        {
            var numberOfRows = 10;            
            var data = _repositoryBase.GetMultiple(x => x
                .FilterBy(y => y.Id <= numberOfRows)
                .IncludeProperties(y => y.SelectProperty(z => z.Address))
            );

            Assert.AreEqual(data.Count(), numberOfRows);
            Assert.IsTrue(data.All(x => x.Address.Any()));
        }

        [TestMethod]
        public void Given_SearchParametersWithFilterAndIncludedPropertiesAndOrder_When_GetMultiple_Then_MultipleDataRetrivedMatchingCriteria()
        {
            var numberOfRows = 10;
            var data = _repositoryBase.GetMultiple(x => x
                .FilterBy(y => y.Id <= numberOfRows)
                .IncludeProperties(y => y.SelectProperty(z => z.Address))
                .OrderBy(y => y.BirthDate)
            );

            Assert.AreEqual(data.Count(), numberOfRows);
            Assert.IsTrue(data.All(x => x.Address.Any()));
        }

        [TestMethod]
        public void Given_SearchParametersWithFilterAndIncludedPropertiesAndOrderDescending_When_GetMultiple_Then_MultipleDataRetrivedMatchingCriteria()
        {
            var numberOfRows = 10;
            var data = _repositoryBase.GetMultiple(x => x
                .FilterBy(y => y.Id <= numberOfRows)
                .IncludeProperties(y => y.SelectProperty(z => z.Address))
                .OrderBy(y => y.BirthDate, OrderType.Descending)
            );

            Assert.AreEqual(data.Count(), numberOfRows);
            Assert.IsTrue(data.All(x => x.Address.Any()));
        }

    }


    public class Person : IKeyable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public IEnumerable<Address> Address { get; set; }
    }


    public class Address : IKeyable<int>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Line { get; set; }
    }

    
}
