using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface INewsRepository
    {
        Task<List<NewsDto>> GetCurrentNewsByCountryAsync(int countryId);
        Task<bool> CreateNewsAsync(NewsEntity news);
    }
}
