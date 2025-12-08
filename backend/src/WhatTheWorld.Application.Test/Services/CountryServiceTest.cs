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
            var expectedCountries = new List<CountryListDto>
            {
                new(1, "Germany", 52.52, 13.405),
                new(2, "United States", 38.895, -77.0369),
                new(3,"France", 48.8566, 2.3522)
            };

            _mockRepository.Setup(r => r.GetAllCountriesAsync())
                          .ReturnsAsync(expectedCountries);

            // Act
            var result = await _service.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Germany", result[0].Name);

            _mockRepository.Verify(r => r.GetAllCountriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            var emptyList = new List<CountryListDto>();
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
                .Select(i => new CountryListDto(i, $"Country{i}", 0.0, 0.0))
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
