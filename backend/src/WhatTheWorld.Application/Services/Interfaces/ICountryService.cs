using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
    }
}
