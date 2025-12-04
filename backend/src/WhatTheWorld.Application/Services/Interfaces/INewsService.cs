using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetCurrentNewsByCountryAsync(int countryId );
        Task<bool> CreateNewsAsync(NewsEntity news);
    }
}
