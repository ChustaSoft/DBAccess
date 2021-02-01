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
            
            var data = repository.Find(MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_Filter_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = repository.Query().First(x => x.Id == MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_FilterAndPropertyToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<City>();

            var data = repository.Query(x => x.Including(y => y.Country))
                .First(x => x.Id == MockDataHelper.STATIC_CITY_ID);

            Assert.NotNull(data);
            Assert.NotNull(data.Country);
        }

        [Fact]
        public void Given_FilterAndNestedPropertiesToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Address>();

            var data = repository.Query(x => x.Including(y => y.City).And(y => y.Country))
                .First(x => x.CityId == MockDataHelper.STATIC_CITY_ID);

            Assert.NotNull(data);
            Assert.NotNull(data.City);
            Assert.NotNull(data.City.Country);
        }

        [Fact]
        public void Given_Filter_When_GetMultiple_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = repository.Query().Where(x => x.Id != MockDataHelper.STATIC_COUNTRY_ID).ToList();

            Assert.NotNull(data);
            Assert.Equal(9, data.Count());
        }

        [Fact]
        public async Task Given_Id_When_GetSingleAsync_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = await repository.FindAsync(MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_FilterAndPagination_When_GetMultiple_Then_DataFiltered()
        {
            int batchSize = 10, page = 0;
            var repository = _unitOfWork.GetRepository<Person>();

            var data = repository.Query()
                .Where(x => x.Id != MockDataHelper.STATIC_PERSON_ID)
                .OrderBy(y => y.BirthDate)
                .Take(batchSize)
                .Skip(page)
                .ToList();

            Assert.NotNull(data);
            Assert.Equal(batchSize, data.Count());
        }

    }
}
