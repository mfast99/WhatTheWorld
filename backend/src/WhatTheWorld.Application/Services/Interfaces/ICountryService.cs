using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<CountryListDto>> GetAllCountriesAsync();
        Task<CountryDto> GetCountryByIdAsync(int id);
    }
}
