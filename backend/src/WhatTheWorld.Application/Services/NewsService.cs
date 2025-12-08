using Microsoft.Extensions.Caching.Memory;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public sealed class NewsService(
        INewsRepository newsRepository,
        IPerplexityService perplexityService,
        IMemoryCache cache) : INewsService
    {
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IPerplexityService _perplexityService = perplexityService;
        private readonly IMemoryCache _cache = cache;

        /// <summary>
        /// Get cached news (FAST)
        /// </summary>
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

        /// <summary>
        /// Check if refresh needed, fetch if yes (SLOW)
        /// </summary>
        public async Task<NewsRefreshResult> RefreshNewsAsync(int countryId)
        {
            var lastFetch = await _newsRepository.GetLastFetchTimeAsync(countryId);
            var now = DateTime.UtcNow;

            if (lastFetch.HasValue && (now - lastFetch.Value).TotalHours < 24)
            {
                Console.WriteLine($"[NewsService] News for country {countryId} already fresh (last fetch: {lastFetch.Value})");

                return new NewsRefreshResult
                {
                    WasRefreshed = false,
                    News = await GetCachedNewsAsync(countryId),
                    LastFetchTime = lastFetch.Value,
                    NextFetchAllowed = lastFetch.Value.AddHours(24)
                };
            }

            Console.WriteLine($"[NewsService] Fetching fresh news for country {countryId}...");

            var freshNews = await _perplexityService.GenerateNewsByCountryIdAsync(countryId);

            if (freshNews.Count == 0)
            {
                Console.WriteLine($"[NewsService] No news received from Perplexity");

                return new NewsRefreshResult
                {
                    WasRefreshed = false,
                    News = await GetCachedNewsAsync(countryId),
                    LastFetchTime = lastFetch,
                    NextFetchAllowed = now.AddHours(1)
                };
            }

            var success = await _newsRepository.CreateNewsAsync(countryId, freshNews);

            if (!success)
            {
                Console.WriteLine($"[NewsService] Failed to save news to database");

                return new NewsRefreshResult
                {
                    WasRefreshed = false,
                    News = await GetCachedNewsAsync(countryId),
                    LastFetchTime = lastFetch,
                    NextFetchAllowed = now.AddHours(1)
                };
            }

            // 5. Invalidate cache
            _cache.Remove($"news_cached_{countryId}");

            // 6. Return fresh news
            var updatedNews = await GetCachedNewsAsync(countryId);

            Console.WriteLine($"[NewsService] Successfully refreshed {freshNews.Count} news for country {countryId}");

            return new NewsRefreshResult
            {
                WasRefreshed = true,
                News = updatedNews,
                LastFetchTime = now,
                NextFetchAllowed = now.AddHours(24)
            };
        }
    }
}

// Result DTO
public sealed record NewsRefreshResult
{
    public required bool WasRefreshed { get; init; }
    public required List<NewsDto> News { get; init; }
    public required DateTime? LastFetchTime { get; init; }
    public required DateTime NextFetchAllowed { get; init; }
}
