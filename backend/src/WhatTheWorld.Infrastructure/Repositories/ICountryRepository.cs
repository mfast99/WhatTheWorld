using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public interface ICountryRepository
    {
        Task<Country?> GetByCodeAsync(string code);
    }
}
