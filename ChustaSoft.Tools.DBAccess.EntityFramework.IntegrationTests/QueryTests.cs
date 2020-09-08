using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Base;
using ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Helpers;
using ChustaSoft.Tools.DBAccess.Examples.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests
{
    public class QueryTests : IntegrationTestBase
    {

        public QueryTests() 
            : base()
        {}


        [Fact]
        public void Given_Id_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();
            
            var data = repository.GetSingle(MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_Filter_When_GetSingle_Then_DataFiltered() 
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = repository.GetSingle(x => x.FilterBy(y => y.Id == MockDataHelper.STATIC_COUNTRY_ID));

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_FilterAndPropertyToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<City>();

            var data = repository.GetSingle(x => x
                .IncludeProperties(y => y.SelectProperty(z => z.Country))
                .FilterBy(y => y.Id == MockDataHelper.STATIC_CITY_ID));

            Assert.NotNull(data);
            Assert.NotNull(data.Country);
        }

        [Fact]
        public void Given_FilterAndNestedPropertiesToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Address>();

            var data = repository.GetSingle(x => x
                .IncludeProperties(y => y.SelectProperty(z => z.City).ThenSelectFromProperty(z => z.City).ThenSelectProperty(z => z.Country).BackToParent())
                .FilterBy(y => y.CityId == MockDataHelper.STATIC_CITY_ID));

            Assert.NotNull(data);
            Assert.NotNull(data.City);
            Assert.NotNull(data.City.Country);
        }

        [Fact]
        public void Given_Filter_When_GetMultiple_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = repository.GetMultiple(x => x.FilterBy(y => y.Id != MockDataHelper.STATIC_COUNTRY_ID)).ToList();

            Assert.NotNull(data);
            Assert.Equal(9, data.Count());
        }

        [Fact]
        public async Task Given_Id_When_GetSingleAsync_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetAsyncRepository<Country>();

            var data = await repository.GetSingleAsync(MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public async Task Given_Filter_When_GetSingleAsync_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetAsyncRepository<Country>();

            var data = await repository.GetSingleAsync(x => x.FilterBy(y => y.Id == MockDataHelper.STATIC_COUNTRY_ID));

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_FilterAndPagination_When_GetMultiple_Then_DataFiltered()
        {
            int batchSize = 10, page = 0;
            var repository = _unitOfWork.GetRepository<Person>();

            var data = repository.GetMultiple(x => x
                .FilterBy(y => y.Id != MockDataHelper.STATIC_PERSON_ID)
                .OrderBy(y => y.BirthDate)
                .Paginate(batchSize, page)
                )
                .ToList();

            Assert.NotNull(data);
            Assert.Equal(batchSize, data.Count());
        }

    }
}
