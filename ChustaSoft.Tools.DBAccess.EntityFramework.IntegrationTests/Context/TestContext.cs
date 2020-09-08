using ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Helpers;
using ChustaSoft.Tools.DBAccess.Examples.Models;
using Microsoft.EntityFrameworkCore;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Context
{
    public class TestContext : DbContext
    {

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }


        public TestContext(DbContextOptions options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            var mockedData = new MockDataHelper();

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable(nameof(Countries));
                entity.HasData(mockedData.Countries);

                entity.HasMany(x => x.Cities).WithOne(x => x.Country).HasForeignKey(x => x.CountryId);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable(nameof(Cities));
                entity.HasData(mockedData.Cities);

                entity.HasMany(x => x.Addresses).WithOne(x => x.City).HasForeignKey(x => x.CityId);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable(nameof(Addresses));
                entity.HasData(mockedData.Addresses);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable(nameof(Persons));
                entity.HasData(mockedData.Persons);

                entity.HasMany(x => x.Addresses).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
            });
        }

    }
}
