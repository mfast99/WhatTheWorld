using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public sealed class NewsRepository(AppDbContext context) : INewsRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<NewsDto>> GetCurrentNewsByCountryAsync(int countryId)
        {
            var cutoffDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)).Date;

            var result = await _context.News
                .Where(n => n.CountryId == countryId && n.PublishedAt.Date > cutoffDate)
                .OrderByDescending(n => n.PublishedAt)
                .Select(n => new NewsDto(
                    n.Id, n.PublishedAt, n.Title, n.Url, n.Source, n.Summary))
                .ToListAsync();

            return result;
        }


        public async Task<bool> CreateNewsAsync(NewsEntity news)
        {
            try
            {
                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
