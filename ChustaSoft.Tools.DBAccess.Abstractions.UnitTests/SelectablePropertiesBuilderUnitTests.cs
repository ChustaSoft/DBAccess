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
            var result = builder.Build();

            Assert.Equal(nameof(Employee.BirthDate), result.Property);
            Assert.Equal(typeof(DateTime), result.Type);
        }

        [Fact]
        public void Given_Queryable_When_ThenAfterIncluding_Then_NestedPropertyAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Addresses);
            var result = builder.Build();

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
            var result = builder.Build();

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
            var result = builder.Build();

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
            var result = builder.Build();

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.Company), result.Nested.Select(x => x.Property));            
            Assert.Contains(typeof(Company), result.Nested.Select(x => x.Type));
            Assert.Contains(nameof(Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Property));
            Assert.Contains(typeof(Address), result.Nested.First(x => x.Property == nameof(Employee.Company)).Nested.Select(x => x.Type));
        }

    }
}
