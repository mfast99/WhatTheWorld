using WhatTheWorld.Domain;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetCurrentWeatherByCountryIdAsync(int countryId);
    }
}
