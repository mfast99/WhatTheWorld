using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<CountryListDto>> GetAllCountriesAsync();
        Task<CountryDto> GetCountryByIdAsync(int Id);
        Task<string> GetCountryNameByIdAsync(int Id);
        Task<int> CreateCountryAsync(CountryEntity country);
        Task<bool> UpdateCountryAsync(CountryEntity country);
        Task<bool> DeleteCountryAsync(string code);
    }
}
