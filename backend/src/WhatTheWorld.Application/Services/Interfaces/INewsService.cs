using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetNewsByCountryIdAsync(int countryId);
    }
}
