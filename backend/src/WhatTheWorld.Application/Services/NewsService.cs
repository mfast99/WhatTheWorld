using Microsoft.Extensions.Caching.Memory;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public sealed class NewsService(INewsRepository newsRepository, IPerplexityService perplexityService, IMemoryCache cache) : INewsService
    {
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IPerplexityService _perplexityService = perplexityService;
        private readonly IMemoryCache _cache = cache;

        public async Task<List<NewsDto>> GetNewsByCountryIdAsync(int countryId)
        {
            var cacheKey = $"news_{countryId}";

            if (_cache.TryGetValue(cacheKey, out List<NewsDto>? cachedNews) && cachedNews?.Count > 0)
                return cachedNews;

            var result = await _newsRepository.GetCurrentNewsByCountryAsync(countryId);

            if (result.Count > 0)
            {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));
                return result;
            }

            var generatedNews = await _perplexityService.GenerateNewsByCountryIdAsync(countryId);

            foreach (var news in generatedNews)
            {
                await _newsRepository.CreateNewsAsync(new NewsEntity
                {
                    CountryId = countryId,
                    PublishedAt = news.PublishedAt,
                    Title = news.Title,
                    Url = news.Url,
                    Source = news.Source,
                    Summary = news.Summary
                });
            }

            result = await _newsRepository.GetCurrentNewsByCountryAsync(countryId);
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
