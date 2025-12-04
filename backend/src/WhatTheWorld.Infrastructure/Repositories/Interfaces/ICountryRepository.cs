using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
        Task<string> GetCountryNameByIdAsync(int countryId);
        Task<CountryDto?> GetCountryByIdAsync(int Id);
        Task<CountryEntity?> GetCountryWithDetailsAsync(string code);
        Task<int> CreateCountryAsync(CountryEntity country);
        Task<bool> UpdateCountryAsync(CountryEntity country);
        Task<bool> DeleteCountryAsync(string code);
    }
}
