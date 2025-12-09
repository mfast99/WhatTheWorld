using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public sealed class NewsRepository(AppDbContext context, ILogger<NewsRepository> logger) : INewsRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<NewsRepository> _logger = logger;

        /// <summary>
        /// Get cached news (last 7 days)
        /// </summary>
        public async Task<List<NewsDto>> GetCachedNewsByCountryAsync(int countryId)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);

            return await _context.News
                .AsNoTracking()
                .Where(n => n.CountryId == countryId && n.PublishedAt > cutoffDate)
                .OrderByDescending(n => n.PublishedAt)
                .Select(n => new NewsDto(
                    n.Id,
                    n.PublishedAt,
                    n.CreatedAt,
                    n.Title,
                    n.Url,
                    n.Source,
                    n.Summary
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Check when news were last created for this country
        /// </summary>
        public async Task<DateTime?> GetLastFetchTimeAsync(int countryId)
        {
            return await _context.News
                .AsNoTracking()
                .Where(n => n.CountryId == countryId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => n.CreatedAt)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Create news items
        /// </summary>
        public async Task<bool> CreateNewsAsync(int countryId, List<NewsDto> newsItems)
        {
            try
            {
                var country = await _context.Countries.FindAsync(countryId);
                if (country == null) return false;

                var now = DateTime.UtcNow;

                var newsEntities = newsItems.Select(dto => new NewsEntity
                {
                    CountryId = countryId,
                    Country = country,
                    PublishedAt = dto.PublishedAt,
                    Title = dto.Title,
                    Url = dto.Url,
                    Source = dto.Source,
                    Summary = dto.Summary,
                    CreatedAt = now
                }).ToList();

                await _context.News.AddRangeAsync(newsEntities);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[NewsRepository] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Delete old news (older than 30 days)
        /// </summary>
        public async Task<int> CleanupOldNewsAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var oldNews = await _context.News
                .Where(n => n.CreatedAt < cutoffDate)
                .ToListAsync();

            if (oldNews.Count == 0)
                return 0;

            _context.News.RemoveRange(oldNews);
            await _context.SaveChangesAsync();

            return oldNews.Count;
        }
    }
}
