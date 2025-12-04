using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly AppDbContext _context;

        public WeatherRepository(AppDbContext context)
        {
            _context = context;
        }

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
