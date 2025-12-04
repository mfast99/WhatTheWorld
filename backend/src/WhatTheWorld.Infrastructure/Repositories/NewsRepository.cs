using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;

        public NewsRepository(AppDbContext context) => _context = context;

        public async Task<List<NewsDto>> GetCurrentNewsByCountryAsync(int countryId)
        {
            var result = _context.News.Where(
                n => n.Country.Id == countryId && 
                n.PublishedAt.Date > DateTime.Now.Subtract(TimeSpan.FromDays(7)))
                .OrderByDescending(n => n.PublishedAt)
                .Select(n => new NewsDto(
                    n.Id, n.PublishedAt, n.Title, n.Url, n.Source, n.Content, n.Summary))
                .ToListAsync();

            return await result;
        }

        public async Task<bool> CreateNewsAsync(NewsEntity news)
        {
            try
            {
                await _context.News.AddAsync(news);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
