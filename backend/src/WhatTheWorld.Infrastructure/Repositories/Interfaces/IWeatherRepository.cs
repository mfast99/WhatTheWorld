using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Repositories.Interfaces
{
    public interface IWeatherRepository
    {
        Task<WeatherDto?> GetCurrentWeatherByCountryAsync(int countryId);
        Task<int> CreateWeatherAsync(WeatherEntity weather);
    }
}
