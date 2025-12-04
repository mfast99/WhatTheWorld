using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Test.Services
{
    public class WeatherServiceTest
    {
        private readonly Mock<IWeatherRepository> _mockWeatherRepo;
        private readonly Mock<ICountryRepository> _mockCountryRepo;
        private readonly Mock<HttpMessageHandler> _mockHttpHandler;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly IMemoryCache _cache;

        public WeatherServiceTest()
        {
            _mockWeatherRepo = new Mock<IWeatherRepository>();
            _mockCountryRepo = new Mock<ICountryRepository>();
            _mockHttpHandler = new Mock<HttpMessageHandler>();
            _mockConfig = new Mock<IConfiguration>();

            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetCurrentWeatherByCountryIdAsync_ReturnsCachedWeather_WhenInCache()
        {
            // Arrange
            var cachedWeather = new WeatherDto(1, DateTime.UtcNow, 15.5, 60.0, "Sunny", "https://icon.com/sunny.png");
            var service = CreateWeatherService(cachedWeather);

            // Act
            var result = await service.GetCurrentWeatherByCountryIdAsync(1);

            // Assert
            Assert.Equal(cachedWeather, result);
            _mockWeatherRepo.Verify(r => r.GetCurrentWeatherByCountryAsync(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCurrentWeatherByCountryIdAsync_ReturnsDbWeather_WhenInDatabase(int countryId)
        {
            // Arrange
            var dbWeather = new WeatherDto(1, DateTime.UtcNow, 20.0, 68.0, "Cloudy", "https://icon.com/cloudy.png");
            _mockWeatherRepo.Setup(r => r.GetCurrentWeatherByCountryAsync(countryId))
                           .ReturnsAsync(dbWeather);

            var service = CreateWeatherService();

            // Act
            var result = await service.GetCurrentWeatherByCountryIdAsync(countryId);

            // Assert
            Assert.Equal(dbWeather, result);
            _mockCountryRepo.Verify(r => r.GetCountryNameByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetCurrentWeatherByCountryIdAsync_ReturnsNull_OnApiFailure()
        {
            // Arrange
            const int countryId = 1;
            _mockWeatherRepo.Setup(r => r.GetCurrentWeatherByCountryAsync(countryId)).ReturnsAsync((WeatherDto?)null);
            _mockCountryRepo.Setup(r => r.GetCountryNameByIdAsync(countryId)).ReturnsAsync("Germany");

            SetupHttpClientFailure();

            var service = CreateWeatherService();

            // Act
            var result = await service.GetCurrentWeatherByCountryIdAsync(countryId);

            // Assert
            Assert.Null(result);
        }

        private WeatherService CreateWeatherService(WeatherDto? cachedWeather = null)
        {
            if (cachedWeather != null)
            {
                _cache.Set($"weather_{1}", cachedWeather, TimeSpan.FromMinutes(10));
            }

            _mockConfig.Setup(c => c["WeatherApiKey"]).Returns("test-api-key-123");

            var httpClient = new HttpClient(_mockHttpHandler.Object);
            return new WeatherService(
                _mockWeatherRepo.Object,
                _mockCountryRepo.Object,
                httpClient,
                _mockConfig.Object,
                _cache);
        }

        private void SetupHttpClientSuccess(WeatherDto expectedWeather)
        {
            var weatherJson = $$"""
            {
                "current": {
                    "temp_c": {{expectedWeather.TempCelsius}},
                    "temp_f": {{expectedWeather.TempFahrenheit}},
                    "condition": {
                        "text": "{{expectedWeather.Description}}",
                        "icon": "//icon.com/{{expectedWeather.Description.ToLower().Replace(" ", "-")}}.png"
                    }
                }
            }
            """;

            _mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("current.json")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(weatherJson, System.Text.Encoding.UTF8, "application/json")
                });
        }

        private void SetupHttpClientFailure()
        {
            _mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }
    }
}
