using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Test.Services
{
    public class CountryServiceTest
    {
        private readonly Mock<ICountryRepository> _mockRepository;
        private readonly CountryService _service;

        public CountryServiceTest()
        {
            _mockRepository = new Mock<ICountryRepository>();
            _service = new CountryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ReturnsCountries_WhenRepositoryReturnsData()
        {
            // Arrange
            var expectedCountries = new List<CountryDto>
            {
                new(1, "DE", "Germany", "Berlin", "🇩🇪", 52.52, 13.405, "Europe", "Western Europe", "83166711", "357022", "UTC+1", "EUR", "German"),
                new(2, "US", "United States", "Washington D.C.", "🇺🇸", 38.895, -77.0369, "Americas", "Northern America", "331002651", "9833515", "EST/PST", "USD", "English"),
                new(3, "FR", "France", "Paris", "🇫🇷", 48.8566, 2.3522, "Europe", "Western Europe", "67318000", "643801", "UTC+1", "EUR", "French")
            };

            _mockRepository.Setup(r => r.GetAllCountriesAsync())
                          .ReturnsAsync(expectedCountries);

            // Act
            var result = await _service.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Germany", result[0].Name);
            Assert.Equal("🇺🇸", result[1].FlagEmoji);

            _mockRepository.Verify(r => r.GetAllCountriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            var emptyList = new List<CountryDto>();
            _mockRepository.Setup(r => r.GetAllCountriesAsync())
                          .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockRepository.Verify(r => r.GetAllCountriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCountriesAsync_PropagatesRepositoryException()
        {
            // Arrange
            var exception = new InvalidOperationException("Database connection failed");
            _mockRepository.Setup(r => r.GetAllCountriesAsync())
                          .ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetAllCountriesAsync());

            _mockRepository.Verify(r => r.GetAllCountriesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(3)]  // Germany
        [InlineData(250)] // Brazil
        public async Task GetAllCountriesAsync_AlwaysCallsRepositoryOnce(int expectedCount)
        {
            // Arrange
            var countries = Enumerable.Range(1, expectedCount)
                .Select(i => new CountryDto(i, $"Code{i}", $"Country{i}", $"Capital{i}", "🏳️", 0.0, 0.0, "Region", "Subregion", "1M", "100k", "UTC", "EUR", "Lang"))
                .ToList();

            _mockRepository.Setup(r => r.GetAllCountriesAsync())
                          .ReturnsAsync(countries);

            // Act
            await _service.GetAllCountriesAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllCountriesAsync(), Times.Once);
        }
    }
}
