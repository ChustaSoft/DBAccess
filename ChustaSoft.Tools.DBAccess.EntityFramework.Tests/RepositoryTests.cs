using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class RepositoryTests : IntegrationTestBase
    {

        public RepositoryTests() 
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

            var data = repository.FromAll().First(x => x.Id == MockDataHelper.STATIC_COUNTRY_ID);

            Assert.NotNull(data);
        }

        [Fact]
        public void Given_FilterAndPropertyToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<City>();

            var data = repository.FromQuery(x => x.Including(y => y.Country))
                .First(x => x.Id == MockDataHelper.STATIC_CITY_ID);

            Assert.NotNull(data);
            Assert.NotNull(data.Country);
        }

        [Fact]
        public void Given_FilterAndNestedPropertiesToInclude_When_GetSingle_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Address>();

            var data = repository.FromQuery(x => x.Including(y => y.City).And(y => y.Country))
                .First(x => x.CityId == MockDataHelper.STATIC_CITY_ID);

            Assert.NotNull(data);
            Assert.NotNull(data.City);
            Assert.NotNull(data.City.Country);
        }

        [Fact]
        public void Given_Filter_When_GetMultiple_Then_DataFiltered()
        {
            var repository = _unitOfWork.GetRepository<Country>();

            var data = repository.FromAll().Where(x => x.Id != MockDataHelper.STATIC_COUNTRY_ID).ToList();

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

            var data = repository.FromAll()
                .Where(x => x.Id != MockDataHelper.STATIC_PERSON_ID)
                .OrderBy(y => y.BirthDate)
                .Take(batchSize)
                .Skip(page)
                .ToList();

            Assert.NotNull(data);
            Assert.Equal(batchSize, data.Count());
        }


        [Fact]
        public void Given_Repository_When_MultipleIncludeWithoutDeepNavigationAndToList_Then_AllPropertiesRetrived()
        {
            var repository = _unitOfWork.GetRepository<Person>();

            var data = repository.FromQuery(x => x.Including(y => y.Origin).Including(y => y.Addresses).Including(y => y.Posts))
                .Where(x => x.Id == MockDataHelper.STATIC_PERSON_ID)
                .OrderByDescending(x => x.BirthDate)
                .ToList();

            Assert.NotNull(data.First().Origin);
            Assert.NotNull(data.First().Addresses);
            Assert.NotNull(data.First().Posts);
        }

        [Fact]
        public void Given_Repository_When_MultipleIncludeWithoutDeepNavigationAndSelect_Then_AllPropertiesRetrived()
        {
            var repository = _unitOfWork.GetRepository<Person>();

            var data = repository.FromQuery(x => x.Including(y => y.Origin).Including(y => y.Addresses).Including(y => y.Posts))
                .Where(x => x.Id == MockDataHelper.STATIC_PERSON_ID)
                .OrderByDescending(x => x.BirthDate)
                .Select(x => new { Name = x.Name, CountryOrigin = x.Origin.Name });

            Assert.NotNull(data);
        }

    }
}
