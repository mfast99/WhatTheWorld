using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface INewsRepository
    {
        Task<List<NewsDto>> GetCachedNewsByCountryAsync(int countryId);
        Task<DateTime?> GetLastFetchTimeAsync(int countryId);
        Task<bool> CreateNewsAsync(int countryId, List<NewsDto> newsItems);
        Task<int> CleanupOldNewsAsync();
    }
}
