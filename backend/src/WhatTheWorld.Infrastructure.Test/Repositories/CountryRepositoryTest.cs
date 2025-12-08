using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories;

namespace WhatTheWorld.Infrastructure.Test.Repositories
{
    public class CountryRepositoryTest(DbContextFixture fixture) : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _fixture = fixture;

        [Fact]
        public async Task GetCountryByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var repo = new CountryRepository(_fixture.Context);

            // Act
            var result = await repo.GetCountryByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCountryAsync_SavesAndReturnsId()
        {
            // Arrange
            var repo = new CountryRepository(_fixture.Context);
            var newCountry = new CountryEntity
            {
                Code = "TEST",
                Name = "Test Country",
                Capital = "Test City",
                FlagEmoji = "🏳️",
                Lat = 0.0,
                Lon = 0.0,
                Region = "Test",
                Subregion = "Test",
                Population = "1000000",
                AreaKm2 = "10000",
                Timezones = "UTC",
                Currencies = "TEST",
                Languages = "Test"
            };

            // Act
            var id = await repo.CreateCountryAsync(newCountry);

            // Assert
            Assert.True(id > 0);
            Assert.Equal(id, newCountry.Id);

            // Verify saved
            var saved = await _fixture.Context.Countries.FindAsync(id);
            Assert.NotNull(saved);
            Assert.Equal("Test Country", saved.Name);
        }

        [Fact]
        public async Task UpdateCountryAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var repo = new CountryRepository(_fixture.Context);
            var nonExistentCountry = new CountryEntity { Id = 999, Name = "Not Found" };

            // Act
            var result = await repo.UpdateCountryAsync(nonExistentCountry);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCountryAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var repo = new CountryRepository(_fixture.Context);

            // Act
            var result = await repo.DeleteCountryAsync("NONEXISTENT");

            // Assert
            Assert.False(result);
        }
    }

    public class DbContextFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public DbContextFixture()
        {
            var options = CreateInMemoryDatabase().Options;

            Context = new AppDbContext(options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
        }

        private static DbContextOptionsBuilder<AppDbContext> CreateInMemoryDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = $"Filename={Guid.NewGuid()}.db";  
            optionsBuilder.UseSqlite(connectionString);
            return optionsBuilder;
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    public static class TestDataSeeder
    {
        public static CountryEntity[] SeedTestData(AppDbContext context)
        {
            var countries = new[]
            {
                new CountryEntity { Id = 1, Code = "DE", Name = "Germany", Capital = "Berlin", FlagEmoji = "🇩🇪", Lat = 52.52, Lon = 13.405, Region = "Europe", Subregion = "Western Europe", Population = "83166711", AreaKm2 = "357022", Timezones = "UTC+1", Currencies = "EUR", Languages = "German" },
                new CountryEntity { Id = 2, Code = "US", Name = "United States", Capital = "Washington D.C.", FlagEmoji = "🇺🇸", Lat = 38.895, Lon = -77.0369, Region = "Americas", Subregion = "Northern America", Population = "331002651", AreaKm2 = "9833515", Timezones = "EST/PST", Currencies = "USD", Languages = "English" },
                new CountryEntity { Id = 3, Code = "FR", Name = "France", Capital = "Paris", FlagEmoji = "🇫🇷", Lat = 48.8566, Lon = 2.3522, Region = "Europe", Subregion = "Western Europe", Population = "67318000", AreaKm2 = "643801", Timezones = "UTC+1", Currencies = "EUR", Languages = "French" }
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();
            return countries;
        }

        public static CountryEntity SeedTestDataWithRelations(AppDbContext context)
        {
            var country = new CountryEntity { Id = 1, Code = "DE", Name = "Germany", Capital = "Berlin", FlagEmoji = "🇩🇪", Lat = 52.52, Lon = 13.405, Region = "Europe", Subregion = "Western Europe", Population = "83166711", AreaKm2 = "357022", Timezones = "UTC+1", Currencies = "EUR", Languages = "German" };
            context.Countries.Add(country);
            context.SaveChanges();
            return country;
        }
    }

}
