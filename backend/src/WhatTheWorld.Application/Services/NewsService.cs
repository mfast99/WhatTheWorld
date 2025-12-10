using Microsoft.Extensions.Caching.Memory;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace WhatTheWorld.Application.Services
{
    public sealed class NewsService(
        INewsRepository newsRepository,
        IPerplexityService perplexityService,
        IMemoryCache cache,
        ILogger<NewsService> logger) : INewsService
    {
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IPerplexityService _perplexityService = perplexityService;
        private readonly IMemoryCache _cache = cache;
        private readonly ILogger<NewsService> _logger = logger;

        public async Task<List<NewsDto>> GetCachedNewsAsync(int countryId)
        {
            var cacheKey = $"news_cached_{countryId}";

            if (_cache.TryGetValue(cacheKey, out List<NewsDto>? cached) && cached != null)
            {
                return cached;
            }

            var news = await _newsRepository.GetCachedNewsByCountryAsync(countryId);

            _cache.Set(cacheKey, news, TimeSpan.FromMinutes(30));

            return news;
        }

        public async Task<NewsRefreshResult> RefreshNewsAsync(int countryId)
        {
            var lastFetch = await _newsRepository.GetLastFetchTimeAsync(countryId);
            var cachedNews = await GetCachedNewsAsync(countryId);
            var now = DateTime.UtcNow;

            var needsRefresh = !lastFetch.HasValue
                || (now - lastFetch.Value).TotalHours >= 24
                || cachedNews.Count == 0;

            if (!needsRefresh)
            {
                return new NewsRefreshResult
                {
                    WasRefreshed = false,
                    News = cachedNews
                };
            }

            var freshNews = await _perplexityService.GenerateNewsByCountryIdAsync(countryId);

            if (freshNews.Count == 0)
            {
                return new NewsRefreshResult
                {
                    WasRefreshed = false,
                    News = cachedNews
                };
            }

            await _newsRepository.CreateNewsAsync(countryId, freshNews);

            _cache.Remove($"news_cached_{countryId}");

            var updatedNews = await GetCachedNewsAsync(countryId);

            return new NewsRefreshResult
            {
                WasRefreshed = true,
                News = updatedNews
            };
        }
    }
}

public sealed record NewsRefreshResult
{
    public required bool WasRefreshed { get; init; }
    public required List<NewsDto> News { get; init; }
}
