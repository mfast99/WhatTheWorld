using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<string> GetCountryNameByIdAsync(int countryId);
        Task<CountryDto?> GetCountryByCodeAsync(string code);
        Task<CountryEntity?> GetCountryWithDetailsAsync(string code);
        Task<int> CreateCountryAsync(CountryEntity country);
        Task<bool> UpdateCountryAsync(CountryEntity country);
        Task<bool> DeleteCountryAsync(string code);
    }
}
