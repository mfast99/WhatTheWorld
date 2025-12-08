using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetCachedNewsAsync(int countryId);
        Task<NewsRefreshResult> RefreshNewsAsync(int countryId);
    }
}
