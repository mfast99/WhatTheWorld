using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPerplexityService _perplexityService;

        public NewsService(INewsRepository newsRepository, IPerplexityService perplexityService)
        {
            _newsRepository = newsRepository;
            _perplexityService = perplexityService;
        }

        public async Task<List<NewsDto>> GetCurrentNewsByCountryAsync(int countryId)
        {
            var result = await _newsRepository.GetCurrentNewsByCountryAsync(countryId);
            if(result.Count == 0)
            {
                var generatedNews = await _perplexityService.GenerateNewsForCountryAsync(countryId);
                foreach(var news in generatedNews)
                {
                    await _newsRepository.CreateNewsAsync(new NewsEntity
                    {
                        CountryId = countryId,
                        PublishedAt = news.PublishedAt,
                        Title = news.Title,
                        Url = news.Url,
                        Source = news.Source,
                        Content = news.Content,
                        Summary = news.Summary
                    });
                }
                result = await _newsRepository.GetCurrentNewsByCountryAsync(countryId);
            }
            return result;
        }

        public async Task<bool> CreateNewsAsync(NewsEntity news)
        {
            return await _newsRepository.CreateNewsAsync(news);
        }
    }
}
