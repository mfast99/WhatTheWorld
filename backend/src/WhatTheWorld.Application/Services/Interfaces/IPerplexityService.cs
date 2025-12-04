using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface IPerplexityService
    {
        Task<List<NewsDto>> GenerateNewsByCountryIdAsync(int countryId);
    }
}
