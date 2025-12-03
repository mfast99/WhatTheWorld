using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services
{
    public interface ICountryService
    {
        Task<Country?> GetByCodeAsync(string countryCode);
    }
}
