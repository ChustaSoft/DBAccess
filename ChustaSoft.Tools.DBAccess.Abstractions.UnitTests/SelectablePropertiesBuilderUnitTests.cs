using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.Abstractions.UnitTests
{
    public class SelectablePropertiesBuilderUnitTests
    {
        [Fact]
        public void Given_Queryable_When_Including_Then_PropertyNameAddedWithoutAggregates()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.BirthDate);
            var result = builder.Build();

            Assert.Equal(nameof(Employee.BirthDate), result.Property);
            Assert.Equal(typeof(DateTime), result.Type);
        }

        [Fact]
        public void Given_Queryable_When_IncludingThen_Then_PropertiesAddedWithNested()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Address);
            var result = builder.Build();

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.Supervisor.Address), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));
        }

        [Fact]
        public void Given_Queryable_When_IncludingAnd_Then_PropertiesAddedWithAggregate()
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
        public void Given_Queryable_When_IncludingThenAnd_Then_BothPropertiesAdded()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).And(x => x.BirthDate).Then(x => x.Address);
            var result = builder.Build();

            Assert.Equal(nameof(Employee.Supervisor), result.Property);
            Assert.Equal(typeof(Employee), result.Type);
            Assert.Contains(nameof(Employee.BirthDate), result.Nested.Select(x => x.Property));
            Assert.Contains(nameof(Employee.Address), result.Nested.Select(x => x.Property));
            Assert.Contains(typeof(DateTime), result.Nested.Select(x => x.Type)); 
            Assert.Contains(typeof(IEnumerable<Address>), result.Nested.Select(x => x.Type));
        }



        //And, getting from current property -> Possible to create a three typed SelectablePropertiesBuilder -> TMain, TCurrent, TProperty

        //Including Extension method using the TMain property, for returning back automatically

    }
}
