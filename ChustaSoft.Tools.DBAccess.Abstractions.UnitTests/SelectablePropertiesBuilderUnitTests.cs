using System;
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
            var result = ((SelectablePropertiesBuilder<Employee, DateTime>)builder).Build();

            Assert.True(result.Root == nameof(Employee.BirthDate));
        }

        [Fact]
        public void Given_Queryable_When_IncludingAnd_Then_PropertiesAddedWithAggregate()
        {
            var data = SelectablePropertiesBuilderTestHelper.GetMockedData();

            var builder = data.Including(x => x.Supervisor).Then(x => x.Address);
            var result = builder.Build();

            Assert.True(result.Root == nameof(Employee.Supervisor));
            Assert.True(result.Nested.ContainsKey(nameof(Employee.Supervisor.Address)));
            Assert.Single(result.Nested.Values);
        }



        //And, getting from current property -> Possible to create a three typed SelectablePropertiesBuilder -> TMain, TCurrent, TProperty

        //Including Extension method using the TMain property, for returning back automatically

    }
}
