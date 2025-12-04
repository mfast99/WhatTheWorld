using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Test.Services
{
    public class NewsServiceTest
    {
        private readonly Mock<INewsRepository> _mockNewsRepo;
        private readonly Mock<IPerplexityService> _mockPerplexityService;
        private readonly IMemoryCache _cache;
        private readonly NewsService _service;

        public NewsServiceTest()
        {
            _mockNewsRepo = new Mock<INewsRepository>();
            _mockPerplexityService = new Mock<IPerplexityService>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new NewsService(_mockNewsRepo.Object, _mockPerplexityService.Object, _cache);
        }

        [Fact]
        public async Task GetNewsByCountryIdAsync_ReturnsCachedNews_WhenInCache()
        {
            // Arrange
            const int countryId = 1;
            var cachedNews = new List<NewsDto>
            {
                new(1, DateTime.UtcNow.AddHours(-1), "Cached News 1", "https://news1.com", "BBC", "Summary 1"),
                new(2, DateTime.UtcNow.AddHours(-2), "Cached News 2", "https://news2.com", "CNN", "Summary 2")
            };

            _cache.Set($"news_{countryId}", cachedNews, TimeSpan.FromHours(1));

            // Act
            var result = await _service.GetNewsByCountryIdAsync(countryId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Cached News 1", result[0].Title);
            _mockNewsRepo.Verify(r => r.GetCurrentNewsByCountryAsync(It.IsAny<int>()), Times.Never);
            _mockPerplexityService.Verify(p => p.GenerateNewsByCountryIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetNewsByCountryIdAsync_ReturnsDatabaseNews_WhenAvailable()
        {
            // Arrange
            const int countryId = 1;
            var dbNews = new List<NewsDto>
            {
                new(3, DateTime.UtcNow.AddDays(-1), "DB News", "https://dbnews.com", "Reuters", "Fresh news")
            };

            _mockNewsRepo.Setup(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync(dbNews);

            // Act
            var result = await _service.GetNewsByCountryIdAsync(countryId);

            // Assert
            Assert.Single(result);
            Assert.Equal("DB News", result[0].Title);
            _mockPerplexityService.Verify(p => p.GenerateNewsByCountryIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetNewsByCountryIdAsync_FetchesPerplexityAndSaves_WhenNoNewsAvailable()
        {
            // Arrange
            const int countryId = 1;

            _mockNewsRepo.Setup(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync(new List<NewsDto>());  

            var perplexityNews = new List<NewsDto>
            {
                new(4, DateTime.UtcNow, "Breaking News", "https://breaking.com", "Perplexity", "AI generated"),
                new(5, DateTime.UtcNow.AddMinutes(-30), "World News", "https://world.com", "Perplexity", "Global events")
            };

            _mockPerplexityService.Setup(p => p.GenerateNewsByCountryIdAsync(countryId))
                                 .ReturnsAsync(perplexityNews);

            _mockNewsRepo.Setup(r => r.CreateNewsAsync(It.IsAny<NewsEntity>()))
                        .ReturnsAsync(true);

            _mockNewsRepo.SetupSequence(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync(new List<NewsDto>())      
                        .ReturnsAsync(perplexityNews);        

            var service = _service;

            // Act
            var result = await service.GetNewsByCountryIdAsync(countryId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Breaking News", result[0].Title);

            _mockPerplexityService.Verify(p => p.GenerateNewsByCountryIdAsync(countryId), Times.Once);
            _mockNewsRepo.Verify(r => r.CreateNewsAsync(It.IsAny<NewsEntity>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetNewsByCountryIdAsync_CachesDatabaseNews()
        {
            // Arrange
            const int countryId = 1;
            var dbNews = new List<NewsDto> { new(6, DateTime.UtcNow, "Fresh DB News", "https://fresh.com", "DW", "Today") };
            _mockNewsRepo.Setup(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync(dbNews);

            // Act 
            var firstResult = await _service.GetNewsByCountryIdAsync(countryId);
            Assert.Single(firstResult);

            var secondResult = await _service.GetNewsByCountryIdAsync(countryId);
            Assert.Single(secondResult);

            // Assert
            _mockNewsRepo.Verify(r => r.GetCurrentNewsByCountryAsync(countryId), Times.Once);
        }

        [Fact]
        public async Task GetNewsByCountryIdAsync_ReturnsEmptyList_WhenPerplexityReturnsEmpty()
        {
            // Arrange
            const int countryId = 1;
            _mockNewsRepo.Setup(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync([]);

            _mockPerplexityService.Setup(p => p.GenerateNewsByCountryIdAsync(countryId))
                                 .ReturnsAsync([]);

            _mockNewsRepo.Setup(r => r.CreateNewsAsync(It.IsAny<NewsEntity>()))
                        .ReturnsAsync(true);

            _mockNewsRepo.Setup(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync([]);

            // Act
            var result = await _service.GetNewsByCountryIdAsync(countryId);

            // Assert
            Assert.Empty(result);
            _mockPerplexityService.Verify(p => p.GenerateNewsByCountryIdAsync(countryId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public async Task GetNewsByCountryIdAsync_CachesPerplexityNews(int countryId)
        {
            // Arrange 
            var perplexityNews = new List<NewsDto>
            {
                new(7, DateTime.UtcNow, "Test News", "https://test.com", "Test", "Cached")
            };
            _mockNewsRepo.SetupSequence(r => r.GetCurrentNewsByCountryAsync(countryId))
                        .ReturnsAsync(new List<NewsDto>())      
                        .ReturnsAsync(perplexityNews);      

            _mockPerplexityService.Setup(p => p.GenerateNewsByCountryIdAsync(countryId))
                                 .ReturnsAsync(perplexityNews);

            _mockNewsRepo.Setup(r => r.CreateNewsAsync(It.IsAny<NewsEntity>()))
                        .ReturnsAsync(true);

            // Act - Erster Call (Perplexity → Cache)
            await _service.GetNewsByCountryIdAsync(countryId);

            var result = await _service.GetNewsByCountryIdAsync(countryId);

            // Assert
            Assert.Single(result);
            Assert.Equal("Test News", result[0].Title);

            _mockPerplexityService.Verify(p => p.GenerateNewsByCountryIdAsync(countryId), Times.Once);
        }
    }
}
