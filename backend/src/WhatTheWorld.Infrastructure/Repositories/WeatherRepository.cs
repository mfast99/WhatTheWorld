using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public sealed class WeatherRepository(AppDbContext context) : IWeatherRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<WeatherDto?> GetCurrentWeatherByCountryAsync(int countryId)
        {
            return _context.Weather.Where(w => w.Country.Id == countryId &&
                                    w.Time >= DateTime.UtcNow.AddHours(-1))
                                   .OrderByDescending(w => w.Time)
                                   .Select(w => new WeatherDto(
                                       w.Id, w.Time, w.TempCelsius, w.TempFahrenheit, w.Description, w.IconUrl))
                                   .FirstOrDefault();
        }

        public async Task<int> CreateWeatherAsync(WeatherEntity weather)
        {
            try
            {
                weather.Country = _context.Countries.Find(weather.CountryId)
                                  ?? throw new InvalidOperationException("Country not found.");
                await _context.Weather.AddAsync(weather);
                await _context.SaveChangesAsync();
                return weather.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
