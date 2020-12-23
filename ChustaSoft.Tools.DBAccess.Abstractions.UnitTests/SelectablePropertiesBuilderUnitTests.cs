using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.Abstractions.UnitTests
{
    public class SelectablePropertiesBuilderUnitTests
    {
        [Fact]
        public void Given_Queryable_When_Including_Then_PropertyNameAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.BirthDate);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.BirthDate), result.Property);
            Assert.Equal(typeof(DateTime), result.Type);
        }

        [Fact]
        public void Given_Queryable_When_ThenAfterIncluding_Then_NestedPropertyAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Addresses);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.Supervisor.Addresses), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_AndAferIncluding_Then_NestedPropertyAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).And(x => x.BirthDate);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.BirthDate), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(DateTime), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_ThenAfterAndAfterIncluding_Then_BothNestedPropertiesAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).And(x => x.BirthDate).Then(x => x.Addresses);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.BirthDate), result.Nested.Select(x => x.Property));
            Assert.Contains(nameof(Employee.Addresses), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(DateTime), result.Nested.Select(x => x.Type)); 
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_DeepenThenAfterThenAfterIncluding_Then_NestedPropertiesBothDeepenAndFlushAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Company).Then(x => x.Address);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.Company), result.Nested.Select(x => x.Property));            
            Assert.Contains(typeof(Company), result.Nested.Select(x => x.Type));
            Assert.Contains(nameof(Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Property));
            Assert.Contains(typeof(Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_DeepenThenAfterAndAfterThenAfterIncluding_Then_NestedPropertiesBothDeepenAndFlushAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Company).And(x => x.BirthDate).Then(x => x.Address);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.Company), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(Company), result.Nested.Select(x => x.Type));
            Assert.Contains(nameof(Company.Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Property));
            Assert.Contains(typeof(Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Type));
            Assert.Contains(nameof(Employee.BirthDate), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(DateTime), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_AndAfterThenWithDuplicatedProperty_Then_ExceptionThrown()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            Assert.Throws<ArgumentException>(() => data.Including(x => x.Supervisor).Then(x => x.Company).And(x => x.Company).Then(x => x.Address));
        }

        [Fact]
        public void Given_Queryable_When_AndAfterAndWithDuplicatedProperty_Then_ExceptionThrown()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            Assert.Throws<ArgumentException>(() => data.Including(x => x.Supervisor).And(x => x.Company).And(x => x.Company));
        }

        [Fact]
        public void Given_Queryable_When_ThenAfterIncludingFromIEnumerable_Then_NestedPropertyAddedFromCollection()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Addresses).Then(x => x.City);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Addresses), result.Property);
            Assert.Equal(typeof(IEnumerable<Address>), result.Type);
            Assert.Contains(nameof(Address.City), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(City), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_ThenAfterThenFromIEnumerable_Then_NestedPropertyAddedFromCollection()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Addresses).Then(x => x.City);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);

            Assert.Contains(nameof(Employee.Addresses), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));

            Assert.Contains(nameof(Address.City), result.Nested.First(x => x.Property == nameof(Employee.Addresses)).Nested.Select(x => x.Property));
            Assert.Contains(typeof(City), result.Nested.First(x => x.Property == nameof(Employee.Addresses)).Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_AndAfterThenFromIEnumerable_Then_NestedPropertyAddedFromCollection()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Addresses).And(x => x.BirthDate);
            var result = builder.Context;

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);

            Assert.Contains(nameof(Employee.Addresses), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));

            Assert.Contains(nameof(Employee.BirthDate), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(DateTime), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_MultipleInclude_Then_AllSiblingsRetrived()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data
                .Including(x => x.Supervisor).Then(x => x.Addresses).And(x => x.BirthDate)
                .Including(x => x.Company).And(x => x.Address);

            var result = builder.Build();

            Assert.Contains(result, x => x.Property == nameof(Employee.Company));
            Assert.Contains(result, x => x.Property == nameof(Employee.Supervisor));
            Assert.Contains(result.First(x => x.Property == nameof(Employee.Supervisor)).Nested, x => x.Property == nameof(Employee.Addresses));
            Assert.Contains(result.First(x => x.Property == nameof(Employee.Supervisor)).Nested, x => x.Property == nameof(Employee.BirthDate));
            Assert.Contains(result.First(x => x.Property == nameof(Employee.Company)).Nested, x => x.Property == nameof(Company.Address));
        }

    }
}
